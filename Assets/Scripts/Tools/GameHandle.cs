using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandle : MonoBehaviour
{
    private Player player;
    private AdManager adManager;
    public static bool gameIsPaused = false;

    [SerializeField] private Button upController;
    [SerializeField] private Button downController;
    [SerializeField] private Button attackController;

    [SerializeField] private SpawnSystem enemySpawnSystem;

    [SerializeField] private Vector3 spawnPlayer = new Vector3(-3f, -0.5f, 0f);
    [SerializeField] private CharacterScriptableObjectAtlas characterScriptableObjectAtlas;
    [SerializeField] private EquipmentScriptableObjectAtlas equipmentScriptableObjectAtlas;

    [SerializeField] private LevelWindow levelWindow;
    [SerializeField] private HeartsHealthVisual heartsHealthVisual;
    [SerializeField] private EquipmentVisual equipmentVisual;
    [SerializeField] private ScoreVisual scoreVisual;

    private LevelSystemAnimated levelSystemAnimated;
    private LevelSystem levelSystem = new LevelSystem();
    private EquipmentSystem equipmentSystem = new EquipmentSystem();
    private ScoreSystem scoreSystem = new ScoreSystem();
    private SaveAndLoadSystem saveAndLoadSystem = new SaveAndLoadSystem();

    private CharacterScriptableObject currentCharacter;
    private EquipmentScriptableObject currentWeapon;
    private float musicVolume, soundEffectsVolume;
    [SerializeField] private GameObject defeatWindow, background, music, pauseMenuWindow, damgeZone;
    private Vector2 damgeZoneTargetPos;
    private int maxEnemiesCounter = 5;
    private float damageZoneSpeed = 0.6f;
    private List<CharacterScriptableObject> ownCharacters;
    private List<EquipmentScriptableObject> ownWeapons;

    private void Start()
    {
        adManager = gameObject.GetComponent<AdManager>();
        adManager.OnSaveProgress += GameHandle_OnSaveProgress;
        adManager.OnGetExtraGold += GameHandle_OnGetExtraGold;

        SaveSystem.Init();
        Load();

        levelSystemAnimated = gameObject.GetComponent<LevelSystemAnimated>();
        player = Player.Create(
            spawnPlayer, currentCharacter, 
            heartsHealthVisual, levelSystem, levelSystemAnimated, currentWeapon, 
            equipmentSystem, scoreSystem);

        SetupControllers();

        player.GetHeartsHealthSystem().OnDead += HeartsHealthSystem_OnDead;

        equipmentVisual.SetEquipmentSystem(equipmentSystem);
        scoreVisual.SetScoreSystem(scoreSystem);
        levelWindow.SetLevelSystem(levelSystem);
        levelSystemAnimated.SetLevelSystem(levelSystem);
        levelWindow.SetLevelSystemAnimated(levelSystemAnimated);

        enemySpawnSystem.StartSpawn();
        Enemy.OnEnemiesCounterChanged += GameHandle_OnEnemyCounterChanged;
        damgeZoneTargetPos = new Vector2(damgeZone.transform.position.x, damgeZone.transform.position.y);
}

    private void Update()
    {
        PauseHandle();
        MoveDamgeZone();
    }

    private void OnDestroy()
    {
        Enemy.OnEnemiesCounterChanged -= GameHandle_OnEnemyCounterChanged;
    }

    #region Game Events
    private void HeartsHealthSystem_OnDead(object sender, System.EventArgs e)
    {
        // player died, save and stop game
        Enemy.DecreseEnemiesCounter(Enemy.GetEnemiesCounter());
        scoreSystem.TrySetHighScore();
        Save();
        enemySpawnSystem.StopSpawn();
        defeatWindow.SetActive(true);
        heartsHealthVisual.gameObject.SetActive(false);
        background.SetActive(false);
        music.SetActive(false);
    }

    public void GameHandle_OnSaveProgress(object sender, System.EventArgs e)
    {
        player.Heal(currentCharacter.maxhealth * 4);
        defeatWindow.SetActive(false);
        heartsHealthVisual.gameObject.SetActive(true);
        background.SetActive(true);
        music.SetActive(true);
        enemySpawnSystem.UnPauseSpawn();
    }

    public void GameHandle_OnGetExtraGold(object sender, System.EventArgs e)
    {
        int currentScore = scoreSystem.GetCurrentScoreAmount();
        int range = 30;
        if (currentScore > 100)
            range = 50;
        else if (currentScore > 250)
            range = 100;
        else if (currentScore > 500)
            range = 150;
        equipmentSystem.AddGoldAmount(Random.Range(10, range));
    }

    private void Load()
    {
        SaveAndLoadSystem.SaveObject saveObject = saveAndLoadSystem.Load();
        equipmentSystem.SetGoldAmount(saveObject.goldAmount);
        scoreSystem.SetHighScoreAmount(saveObject.highScoreAmount);
        levelSystem.SetExperience(saveObject.experience);
        levelSystem.SetLevel(saveObject.level);
        currentCharacter = saveObject.currentCharacter;
        currentWeapon = saveObject.currentWeapon;
        musicVolume = saveObject.musicVolume;
        soundEffectsVolume = saveObject.soundEffectsVolume;
        ownCharacters = saveObject.ownCharacters;
        ownWeapons = saveObject.ownWeapons;
    }

    private void Save()
    {
        saveAndLoadSystem.Save(
            equipmentSystem.GetGoldAmount(),
            scoreSystem.GetHighScoreAmount(),
            levelSystem.GetLevelNumber(),
            levelSystem.GetExperience(),
            currentCharacter,
            currentWeapon,
            musicVolume,
            soundEffectsVolume,
            ownCharacters,
            ownWeapons
        );
    }

    private void SetupControllers()
    {
        upController.onClick.AddListener(player.onUpButtonClicked);
        downController.onClick.AddListener(player.onDownButtonClicked);
        attackController.onClick.AddListener(player.onAttackClicked);
    }
    #endregion

    #region Damage Zone
    public void MoveDamgeZone()
    {
        if (damgeZoneTargetPos != new Vector2(damgeZone.transform.position.x, damgeZone.transform.position.y))
            damgeZone.transform.position = Vector2.MoveTowards(damgeZone.transform.position, damgeZoneTargetPos, damageZoneSpeed * Time.deltaTime);
    }

    private void GameHandle_OnEnemyCounterChanged(object sender, System.EventArgs e)
    {
        if (Enemy.GetEnemiesCounter() >= maxEnemiesCounter)
        {
            Enemy.DecreseEnemiesCounter(3);
            StartCoroutine(DamageZoneAniamtion());
        }
        else
            damgeZoneTargetPos.x = -13.5f + (float)Enemy.GetEnemiesCounter() * 0.65f;
        Debug.Log("Target Pos: " + damgeZoneTargetPos + " counter: " + Enemy.GetEnemiesCounter());
    }
    private IEnumerator DamageZoneAniamtion()
    {
        damageZoneSpeed *= 2;
        damgeZoneTargetPos.x = -13.5f + (maxEnemiesCounter + 1) * 0.65f;
        yield return new WaitForSeconds(0.2f);
        player.GetHeartsHealthSystem().Damage(1);
        damageZoneSpeed /= 2;
        damgeZoneTargetPos.x = -13.5f + (float)Enemy.GetEnemiesCounter() * 0.65f;
    }
    #endregion

    #region Pause Menu
    private void PauseHandle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuWindow.SetActive(false);
        background.SetActive(true);
        music.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        scoreSystem.TrySetHighScore();
        Save();
        pauseMenuWindow.SetActive(true);
        background.SetActive(false);
        music.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    #endregion
}
