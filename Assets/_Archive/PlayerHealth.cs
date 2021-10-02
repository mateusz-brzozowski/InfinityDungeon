using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //private int health = 1;
    private int armorModifier = 0;
    private int weaponArmorModifier = 0;
    //private bool alive = false;

    private GameObject bloodEffect = null;


    public void TakeDamage(int damage)
    {
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        damage -= armorModifier;
        damage -= weaponArmorModifier;
        if (damage > 0)
        {
            //health -= damage;
            DamagePopup.Create(transform.position, damage, false);
        }
    }
}
