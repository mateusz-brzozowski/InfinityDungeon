using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BounceInBox : MonoBehaviour
{
    [SerializeField] private float width, height;
    private float imageWidth, imageHeight;
    private float x, y;
    private float xSpeed, ySpeed;
    private RectTransform imageTransform;

    private void Start()
    {
        x = Random.Range(0, width + 1);
        y = Random.Range(0, height + 1);
        xSpeed = 40f;
        ySpeed = 40f;
        imageTransform = gameObject.GetComponent<RectTransform>();
        imageWidth = imageTransform.rect.width;
        imageHeight = imageTransform.rect.height;
    }

    private void Update()
    {
        x = x + xSpeed * Time.deltaTime;
        y = y + ySpeed * Time.deltaTime;

        if (x + imageWidth >= width)
        {
            xSpeed = -xSpeed;
            x = width - imageWidth;
        }
        else if (x <= 0)
        {
            xSpeed = -xSpeed;
            x = 0;
        }

        if (y + imageHeight >= height)
        {
            ySpeed = -ySpeed;
            y = height - imageHeight;
        }
        else if (y <= 0)
        {
            ySpeed = -ySpeed;
            y = 0;
        }
        imageTransform.anchoredPosition = new Vector2(x, y);
    }
}
