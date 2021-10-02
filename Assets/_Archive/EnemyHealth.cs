using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //private string enemyName = "";
    private int health = 1;
    //private bool alive = false;
    //private int armorModifier = 0;

    //private GameObject bloodEffect = null;

    //private int experience = 0;

    public void TakeDamage(int damage, float thrust)
    {
        //Instantiate(bloodEffect, transform.position, Quaternion.identity);

        bool isCriticalHit = Random.Range(0, 100) < 30;

        if (isCriticalHit)
            damage *= 2;
        //damage -= armorModifier;
        if (damage > 0)
        {
            health -= damage;
            gameObject.GetComponent<EnemyMovement>().Knockback(thrust);
            DamagePopup.Create(transform.position, damage, isCriticalHit);
        }
        if (health <= 0)
        {
            // TO CHANGE
            //GameObject.FindGameObjectWithTag("GameController").GetComponent<Testing>().levelSystem.AddExperience(experience, "Kill " + enemyName);
            Destroy(gameObject);
        }
    }
}
