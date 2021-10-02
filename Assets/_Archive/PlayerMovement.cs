using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 targetPos;
    const float yIncrement = 1;

    const float speed = 50f;
    const float maxHeight = 0.5f;
    const float minHeight = -1.5f;

    private GameObject effect = null;

    private Animator camAnim = null;

    private void Start()
    {
        targetPos = transform.position;
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        //effect = gameObject.GetComponent<Player>().GetOnMoveEffectFromAtlas(-1);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < maxHeight)
        {
            camAnim.SetTrigger("shake");
            Instantiate(effect, transform.position, Quaternion.identity);
            targetPos = new Vector2(transform.position.x, transform.position.y + yIncrement);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > minHeight)
        {
            camAnim.SetTrigger("shake");
            Instantiate(effect, transform.position, Quaternion.identity);
            targetPos = new Vector2(transform.position.x, transform.position.y - yIncrement);
        }
    }

    public void onUpButtonClicked()
    {
        if (transform.position.y < maxHeight)
        {
            camAnim.SetTrigger("shake");
            Instantiate(effect, transform.position, Quaternion.identity);
            targetPos = new Vector2(transform.position.x, transform.position.y + yIncrement);
        }
    }

    public void onDownButtonClicked()
    {
        if (transform.position.y > minHeight)
        {
            camAnim.SetTrigger("shake");
            Instantiate(effect, transform.position, Quaternion.identity);
            targetPos = new Vector2(transform.position.x, transform.position.y - yIncrement);
        }
    }
}
