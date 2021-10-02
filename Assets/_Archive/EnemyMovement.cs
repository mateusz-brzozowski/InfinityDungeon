using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float speed = 1.5f;

    private Rigidbody2D enemyRigidbody = null;
    private Vector3 moveDirection = new Vector3(1f, 0f);
    private float knockTime = 0.3f;

    private void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void Knockback(float thrust)
    {
        enemyRigidbody.isKinematic = false;
        enemyRigidbody.AddForce(moveDirection.normalized * thrust, ForceMode2D.Impulse);
        StartCoroutine(KnockCo(enemyRigidbody));
    }

    private IEnumerator KnockCo(Rigidbody2D enemyRigidbody)
    {
        if(enemyRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemyRigidbody.velocity = Vector2.zero;
            enemyRigidbody.isKinematic = true;
        }
    }
}
