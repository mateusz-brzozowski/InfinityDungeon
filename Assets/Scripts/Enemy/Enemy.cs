using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position, EnemyScriptableObject enemyScriptableObject, int valuesMultiplayer)
    {
        Transform enemyTransform = Instantiate(Resources.Load<Transform>("Prefabs/Enemy"), position, Quaternion.identity);
        Enemy enemyHandler = enemyTransform.GetComponent<Enemy>();
        enemyHandler.Setup(enemyScriptableObject, valuesMultiplayer);
        return enemyHandler;
    }

    public static event EventHandler OnEnemiesCounterChanged;
    private static int enemiesCounter = 0;
    public static int GetEnemiesCounter() { return enemiesCounter; }
    public static void DecreseEnemiesCounter(int decreseValue) { enemiesCounter -= decreseValue; }

    private EnemyScriptableObject enemyScriptableObject;
    private HealthSystem healthSystem;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D enemyRigidbody;
    private int valuesMultiplayer;

    public void Setup(EnemyScriptableObject enemyScriptableObject, int valuesMultiplayer)
    {
        this.enemyScriptableObject = enemyScriptableObject;
        this.valuesMultiplayer = valuesMultiplayer;

        healthSystem = new HealthSystem(enemyScriptableObject.maxhealth);
        healthSystem.OnDead += HealthSystem_OnDead;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveTransform();
        MoveAnimation();
    }

    #region Enemy Getters
    public int GetExperience()
    {
        return enemyScriptableObject.experience * valuesMultiplayer;
    }
    public int GetGoldAmount()
    {
        return enemyScriptableObject.goldAmount * valuesMultiplayer;
    }
    public int GetScoreAmount()
    {
        return enemyScriptableObject.scoreAmount;
    }
    public string GetName()
    {
        return enemyScriptableObject.name;
    }
    #endregion

    #region Enemy Movement
    private float speed = 1.5f;
    private Vector3 moveDirection = new Vector3(1f, 0f);
    private float knockTime = 0.3f;

    private void MoveTransform()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }

    public void Knockback(float thrust)
    {
        enemyRigidbody.isKinematic = false;
        enemyRigidbody.AddForce(moveDirection.normalized * thrust, ForceMode2D.Impulse);
        StartCoroutine(KnockCo(enemyRigidbody));
    }

    private IEnumerator KnockCo(Rigidbody2D enemyRigidbody)
    {
        if (enemyRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemyRigidbody.velocity = Vector2.zero;
            enemyRigidbody.isKinematic = true;
        }
    }
    #endregion

    #region Enemy Animation
    private int currentFrame = 0;
    private float animationTimer = 0f;
    const float framerate = 0.2f;

    private void MoveAnimation()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer >= framerate)
        {
            animationTimer -= framerate;
            currentFrame = (currentFrame + 1) % enemyScriptableObject.runSprites.Length;
            spriteRenderer.sprite = enemyScriptableObject.runSprites[currentFrame];
        }
    }
    #endregion

    #region Enemy Health
    public void Damage(int damage, float thrust)
    {
        bool isCriticalHit = UnityEngine.Random.Range(0, 100) < 30;
        if (isCriticalHit)
            damage *= 2;
        if(UnityEngine.Random.Range(0, 100) < 30)
            damage -= enemyScriptableObject.armorModifier * valuesMultiplayer;
        if (damage > 0)
        {
            healthSystem.Damage(damage);
            DamagePopup.Create(transform.position, damage, isCriticalHit);
        }
        else
            DamagePopup.Create(transform.position, 0);
        Knockback(thrust);
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
    #endregion

    #region Enemy Events
    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        // Enemy Die, do sth
        if (enemiesCounter > 0)
        {
            enemiesCounter--;
            OnEnemiesCounterChanged?.Invoke(this, EventArgs.Empty);
        }
        Destroy(gameObject);
        SpawnDieParticleEffect();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        // Enemy take damage, do sth
        CircleEffect.Create(transform.position, Color.red);
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        // Enemy healed, do sth
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        // Enemy health changed, do sth
    }
    #endregion

    #region Enemy Attack and Destroyer
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // TODO: Find better place for this code or make it better
        // Ememy Destroyer
        if (collider.CompareTag("Destroyer"))
        {
            enemiesCounter++;
            OnEnemiesCounterChanged?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
        // Enemy Attack
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(enemyScriptableObject.damageModifier * valuesMultiplayer);
            Destroy(gameObject);
        }
    }
    #endregion

    #region Enemy Effects
    private void SpawnDieParticleEffect()
    {
        Instantiate(enemyScriptableObject.dieEffect, transform.position, Quaternion.identity);
    }
    #endregion
}
