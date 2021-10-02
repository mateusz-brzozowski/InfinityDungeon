using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyAnimation : MonoBehaviour
{
    private Sprite[] frameArray = null;
    private int currentFrame;
    private float timer;
    const float framerate = 0.2f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //frameArray = gameObject.GetComponent<EnemyController>().GetArrayFromAtlas(-1);
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % frameArray.Length;
            spriteRenderer.sprite = frameArray[currentFrame];
        }
    }
}
