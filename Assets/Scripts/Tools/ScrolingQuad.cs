using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolingQuad : MonoBehaviour
{
    Material material;
    Vector2 offset;

    public float x_velocity, y_velocity;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        offset = new Vector2(x_velocity, y_velocity);
    }

    private void Update()
    {

        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
