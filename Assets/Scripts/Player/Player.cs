using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Create(
        Vector3 position, CharacterScriptableObject characterScriptableObject, 
        HeartsHealthVisual heartsHealthVisual, LevelSystem levelSystem, LevelSystemAnimated levelSystemAnimated,
        EquipmentScriptableObject equipmentScriptableObject, EquipmentSystem equipmentSystem, ScoreSystem scoreSystem)
    {
        Transform playerTransform = Instantiate(Resources.Load<Transform>("Prefabs/Player"), position, Quaternion.identity);
        Player playerHandler = playerTransform.GetComponent<Player>();
        playerHandler.Setup(
            characterScriptableObject, heartsHealthVisual, levelSystem, 
            levelSystemAnimated, equipmentScriptableObject, equipmentSystem, scoreSystem);
        return playerHandler;
    }

    private CharacterScriptableObject characterScriptableObject;
    private Weapon weapon;

    private LevelSystemAnimated levelSystemAnimated;
    private LevelSystem levelSystem;

    private HeartsHealthVisual heartsHealthVisual;
    private HeartsHealthSystem heartsHealthSystem;

    private EquipmentSystem equipmentSystem;
    private ScoreSystem scoreSystem;

    private SpriteRenderer spriteRenderer;
    private Animator camAnim;
    private Vector2 targetPos;

    public void Setup(CharacterScriptableObject characterScriptableObject, HeartsHealthVisual heartsHealthVisual, 
        LevelSystem levelSystem, LevelSystemAnimated levelSystemAnimated, EquipmentScriptableObject equipmentScriptableObject,
        EquipmentSystem equipmentSystem, ScoreSystem scoreSystem)
    {
        this.characterScriptableObject = characterScriptableObject;
        weapon = Weapon.Create(transform, equipmentScriptableObject);

        this.heartsHealthVisual = heartsHealthVisual;
        heartsHealthSystem = new HeartsHealthSystem(characterScriptableObject.maxhealth);
        heartsHealthVisual.SetHeartsHealthSystem(heartsHealthSystem);
        heartsHealthSystem.OnDamged += HeartsHealthSystem_OnDamaged;
        heartsHealthSystem.OnHealed += HeartsHealthSystem_OnHealed;
        heartsHealthSystem.OnDead += HeartsHealthSystem_OnDead;

        this.levelSystem = levelSystem;
        this.levelSystemAnimated = levelSystemAnimated;
        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;

        this.equipmentSystem = equipmentSystem;
        this.scoreSystem = scoreSystem;
    }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        targetPos = transform.position;
    }

    private void Update()
    {
        PlayerUpDownMovemnet();
        AttackHandle();
        MoveAnimation();
    }

    #region Player Getters
    public HeartsHealthSystem GetHeartsHealthSystem()
    {
        return heartsHealthSystem;
    }

    public LevelSystem GetLevelSystem()
    {
        return levelSystem;
    }
    #endregion

    #region Player Movement
    const float yIncrement = 1;
    const float speed = 50f;
    const float maxHeight = 0.5f;
    const float minHeight = -1.5f;
    

    private void PlayerUpDownMovemnet()
    {
        if (targetPos != new Vector2(transform.position.x, transform.position.y))
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < maxHeight)
        {
            // TODO: cam animator
            camAnim.SetTrigger("shake");
            SpawnMoveParticleEffect();
            targetPos = new Vector2(transform.position.x, transform.position.y + yIncrement);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > minHeight)
        {
            // TODO: cam animator
            camAnim.SetTrigger("shake");
            SpawnMoveParticleEffect();
            targetPos = new Vector2(transform.position.x, transform.position.y - yIncrement);
        }
    }
    public void onUpButtonClicked()
    {
        if (transform.position.y < maxHeight)
        {
            // TODO: cam animator
            camAnim.SetTrigger("shake");
            SpawnMoveParticleEffect();
            targetPos = new Vector2(transform.position.x, transform.position.y + yIncrement);
        }
    }

    public void onDownButtonClicked()
    {
        if (transform.position.y > minHeight)
        {
            // TODO: cam animator
            camAnim.SetTrigger("shake");
            SpawnMoveParticleEffect();
            targetPos = new Vector2(transform.position.x, transform.position.y - yIncrement);
        }
    }
    #endregion

    #region Player Attack
    private float timeBtwAttack = 0f;
    private const float startTimeBtwAttack = 0.1f;

    [SerializeField] private LayerMask whatIsEnemies;
    //[SerializeField] private float attackRange = 0.43f;
    
    private const float attackRangeX = 0.94f;
    private const float attackRangeY = 0.63f;
    private Vector3 offset = new Vector3(1.0f, -0.5f);

    private bool attack;
    private bool autoAttack;
    private void AttackHandle()
    {
        if (timeBtwAttack <= 0)
        {
             if (attack || Input.GetKey(KeyCode.Space) || autoAttack)
            {
                // TODO: SET weapon and camera TRIGGER or create event
                camAnim.SetTrigger("shake");
                weapon.OnAttack();
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(
                    transform.position + offset + new Vector3(weapon.GetRangeModifier() / 2, 0f), 
                    new Vector2(attackRangeX + weapon.GetRangeModifier(), attackRangeY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    Enemy enemyHandler = enemiesToDamage[i].GetComponent<Enemy>();
                    enemyHandler.Damage(weapon.GetDamageModifier(), weapon.GetThrust());
                    if (enemyHandler.IsDead())
                    {
                        levelSystem.AddExperience(enemyHandler.GetExperience(), "KILL " + enemyHandler.GetName());
                        equipmentSystem.AddGoldAmount(enemyHandler.GetGoldAmount());
                        scoreSystem.AddCurrentScoreAmount(enemyHandler.GetScoreAmount());
                    }
                }
                attack = false;
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
            timeBtwAttack -= Time.deltaTime;
    }
    public void onAttackClicked()
    {
        attack = true;
    }

    public void SetAutoAttack(float autoAttackTime)
    {
        StartCoroutine(AutoAttackTimer(autoAttackTime));
    }

    private IEnumerator AutoAttackTimer(float autoAttackTime)
    {
        autoAttack = true;
        spriteRenderer.color = Color.magenta;
        yield return new WaitForSeconds(autoAttackTime);
        spriteRenderer.color = Color.white;
        autoAttack = false;
    }
    #endregion

    #region Player Effects
    private void SpawnMoveParticleEffect()
    {
        Instantiate(characterScriptableObject.onMoveEffect, transform.position, Quaternion.identity);
    }
    #endregion

    #region Player Animations
    private int currentFrame;
    private float timer;
    const float framerate = 0.2f;

    private void MoveAnimation()
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % characterScriptableObject.runSprites.Length;
            spriteRenderer.sprite = characterScriptableObject.runSprites[currentFrame];
        }
    }
    #endregion

    #region Player Events
    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        // level up, do sth
    }
    private void HeartsHealthSystem_OnDead(object sender, System.EventArgs e)
    {
        // player died
        CircleEffect.Create(transform.position, Color.red);
    }

    private void HeartsHealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        // Player damaged
        CircleEffect.Create(transform.position, Color.red);
    }

    private void HeartsHealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        // Player healed

    }
    #endregion

    #region Player Health
    private bool isShield;
    public void Damage(int damage)
    {
        if (isShield)
            return;
        if (UnityEngine.Random.Range(0, 100) < 30)
            damage -= characterScriptableObject.armorModifier;
        if (UnityEngine.Random.Range(0, 100) < 30)
            damage -= weapon.GetArmorModifier();
        if (damage > 0)
        {
            heartsHealthSystem.Damage(damage);
            DamagePopup.Create(transform.position, damage);
        }
        else
            DamagePopup.Create(transform.position, 0);

    }

    public void Heal(int healAmount)
    {
        heartsHealthSystem.Heal(healAmount);
    }

    public void SetShield(float shieldTime)
    {
        StartCoroutine(ShiledTimer(shieldTime));
    }

    private IEnumerator ShiledTimer(float shieldTime)
    {
        isShield = true;
        spriteRenderer.color = Color.cyan;
        yield return new WaitForSeconds(shieldTime);
        spriteRenderer.color = Color.white;
        isShield = false;
    }
    #endregion
}