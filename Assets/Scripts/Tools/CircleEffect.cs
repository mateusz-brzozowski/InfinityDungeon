using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEffect : MonoBehaviour
{
    public static CircleEffect Create(Vector3 position, Color color)
    {
        Transform circleEffectTransform = Instantiate(Resources.Load<Transform>("Prefabs/CircleEffect"), position, Quaternion.identity);
        CircleEffect circleEffect = circleEffectTransform.GetComponent<CircleEffect>();
        circleEffect.Setup(color);
        return circleEffect;
    }

    private new ParticleSystem particleSystem;
    private float timeLeft;

    private void Awake()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    public void Setup(Color color)
    {
        color.a = 20;
        //particleSystem.startColor = color;
        timeLeft = particleSystem.duration;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
