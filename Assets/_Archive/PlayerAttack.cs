using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack = 0f;
    [SerializeField] private float startTimeBtwAttack = 0.1f;

    [SerializeField] private LayerMask whatIsEnemies;
    //[SerializeField] private float attackRange = 0.43f;
    private int damage = 1;
    private float thrust = 3;

    private Animator camAnim;
    private Animator playerAnim;
    [SerializeField] private float attackRangeX = 0.94f;
    [SerializeField] private float attackRangeY = 0.63f;
    [SerializeField] private Vector3 offset = new Vector3(1.0f, -0.5f);
    private float rangeModifier = 0;

    private bool attack;

    private void Start()
    {
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        playerAnim = GetComponent<Animator>();
/*        damage = gameObject.GetComponent<Weapon>().GetDamageModifierFromAtlas(-1);
        thrust = gameObject.GetComponent<Weapon>().GetThrustFromAtlas(-1);
        rangeModifier = gameObject.GetComponent<Weapon>().GetRangeFromAtlas(-1);*/
    }

    private void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (attack || Input.GetKey(KeyCode.Space))
            {
                camAnim.SetTrigger("shake");
                playerAnim.SetTrigger("attack");
                //Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(
                    transform.position + offset + new Vector3(rangeModifier/2, 0f), new Vector2(attackRangeX + rangeModifier, attackRangeY),0, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(damage, thrust);
                    enemiesToDamage[i].GetComponent<Enemy>().Damage(damage, thrust);
;               }
                attack = false;
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
            timeBtwAttack -= Time.deltaTime;
    }
    public void onAttackClicked()
    {
        attack = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireCube(transform.position + offset + new Vector3(rangeModifier/2, 0f), new Vector3(attackRangeX + rangeModifier, attackRangeY, 1));
    }
}
