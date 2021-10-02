using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandle : MonoBehaviour
{
    private LevelSystem levelSystem = new LevelSystem();
    private EquipmentSystem equipmentSystem = new EquipmentSystem();
    private ScoreSystem scoreSystem = new ScoreSystem();
    private SaveAndLoadSystem saveAndLoadSystem = new SaveAndLoadSystem();

    [SerializeField] private WeaponWindow weaponWindow;
    [SerializeField] private CharacterWindow characterWindow;
    [SerializeField] private OptionsWindow optionsWindow;
    [SerializeField] private GoldStoreWindow goldStoreWindow;

    [SerializeField] private CharacterScriptableObjectAtlas allCharacters;
    [SerializeField] private CharacterScriptableObjectAtlas ownCharacters;

    [SerializeField] private EquipmentScriptableObjectAtlas allWeapons;
    [SerializeField] private EquipmentScriptableObjectAtlas ownWeapons;

    private CharacterScriptableObject currentCharacter;
    private EquipmentScriptableObject currentWeapon;

    [SerializeField] private GameObject loadingCutscene;

    private void Awake()
    {
        int isSceneCahnged = DefeatWindow.GetIsSceneChanged();
        if (isSceneCahnged == 1)
            loadingCutscene.SetActive(false);
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        SaveSystem.Init();
        SaveAndLoadSystem.SaveObject saveObject = saveAndLoadSystem.Load();
        equipmentSystem.SetGoldAmount(saveObject.goldAmount);
        scoreSystem.SetHighScoreAmount(saveObject.highScoreAmount);
        levelSystem.SetExperience(saveObject.experience);
        levelSystem.SetLevel(saveObject.level);
        if (saveObject.currentCharacter)
            currentCharacter = saveObject.currentCharacter;
        else
            currentCharacter = allCharacters.GetCharacterAtIndex(0);
        if (saveObject.currentWeapon)
            currentWeapon = saveObject.currentWeapon;
        else
            currentWeapon = allWeapons.GetEquipmentAtIndex(0);
        optionsWindow.Setup(saveObject.musicVolume, saveObject.soundEffectsVolume);
        if(saveObject.ownCharacters.Count > 0)
            ownCharacters.SetList(saveObject.ownCharacters);
        if(saveObject.ownWeapons.Count > 0)
            ownWeapons.SetList(saveObject.ownWeapons);

        goldStoreWindow.Setup();
        goldStoreWindow.SetEquipmentSystem(equipmentSystem);
        equipmentSystem.OnGoldAmountChanged += MainMenuHandle_OnGoldAmountChanged;

        characterWindow.Setup();
        characterWindow.SetLevelSystem(levelSystem);
        characterWindow.SetEquipmentSystem(equipmentSystem);
        characterWindow.SetCharacterScriptableObject(allCharacters, ownCharacters, currentCharacter);

        characterWindow.OnCurrentCharacterChanged += MainMenuHandle_OnCurrentCharacterChanged;

        weaponWindow.Setup();
        weaponWindow.SetLevelSystem(levelSystem);
        weaponWindow.SetEquipmentSystem(equipmentSystem);
        weaponWindow.SetCharacterScriptableObject(allWeapons, ownWeapons, currentWeapon);

        weaponWindow.OnCurrentWeaponChanged += MainMenuHandle_OnCurrentWeaponChanged;

        optionsWindow.OnVolumeChaned += MainMenuHandle_OnVolumeChaned;
        Save();
    }

    #region Main Menu Events
    private void MainMenuHandle_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        Save();
    }

    private void MainMenuHandle_OnCurrentCharacterChanged(object sender, System.EventArgs e)
    {
        Save();
    }

    private void MainMenuHandle_OnCurrentWeaponChanged(object sender, System.EventArgs e)
    {
        Save();
    }

    private void MainMenuHandle_OnVolumeChaned(object sender, System.EventArgs e)
    {
        Save();
    }
    #endregion

    private void Save()
    {
        saveAndLoadSystem.Save(
            equipmentSystem.GetGoldAmount(),
            scoreSystem.GetHighScoreAmount(),
            levelSystem.GetLevelNumber(),
            levelSystem.GetExperience(),
            characterWindow.GetCurrentCharacter(),
            weaponWindow.GetCurrentWeapon(),
            optionsWindow.GetMusicVolume(),
            optionsWindow.GetSoundEffectsVolume(),
            ownCharacters.GetList(),
            ownWeapons.GetList()
        );
    }
}
