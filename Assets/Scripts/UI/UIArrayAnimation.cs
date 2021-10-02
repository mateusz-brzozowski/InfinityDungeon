using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIArrayAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] frameArray = null;
    private int currentFrame;
    private float timer;
    const float framerate = 0.2f;
    private Image imageRenderer;

    private void Start()
    {
        imageRenderer = gameObject.GetComponent<Image>();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % frameArray.Length;
            imageRenderer.sprite = frameArray[currentFrame];
        }
    }
}
