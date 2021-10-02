using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public enum PotionType { Heal, Experience, Shield, AutoAttack };
    public static Potion Create(Vector3 position, PotionType potionType)
    {
        Transform potionTransform = Instantiate(Resources.Load<Transform>("Prefabs/Potion"), position, Quaternion.identity);
        Potion potionHandler = potionTransform.GetComponent<Potion>();
        potionHandler.Setup(potionType);
        return potionHandler;
    }

    PotionType potionType;
    private int value;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite healPotion, experiencePotion, shildPotion, autoAttackPotion;

    public void Setup(PotionType potionType)
    {
        this.potionType = potionType;
        switch (potionType)
        {
            case PotionType.Heal: spriteRenderer.sprite = healPotion; value = Random.Range(1, 4); break;
            case PotionType.Experience: spriteRenderer.sprite = experiencePotion; value = Random.Range(30, 90); break;
            case PotionType.Shield: spriteRenderer.sprite = shildPotion; value = Random.Range(3, 10); break;
            case PotionType.AutoAttack: spriteRenderer.sprite = autoAttackPotion; value = Random.Range(3, 8); break;
        }
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            switch(potionType)
            {
                case PotionType.Heal: player.Heal(value); break;
                case PotionType.Experience: player.GetLevelSystem().AddExperience(value, "Drink Potion"); break;
                case PotionType.Shield: player.SetShield(value); break;
                case PotionType.AutoAttack: player.SetAutoAttack(value); break;
            }
            Destroy(gameObject);
        }
    }
}
