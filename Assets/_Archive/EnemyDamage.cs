using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private int damage = 1;

    private void Start()
    {
        //damage = gameObject.GetComponent<EnemyController>().GetDamageModifierFromAtlas(-1);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(damage);
            Destroy(gameObject);
        }
    }
}
