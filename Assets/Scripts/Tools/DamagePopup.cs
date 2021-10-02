using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit = false)
    {
        Transform damagePopupTransform = Instantiate(Resources.Load<Transform>("Prefabs/DamagePopup"), position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);
        return damagePopup;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 0.5f;

    private TextMeshPro textMesh;
    private float disapperTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        if (damageAmount <= 0)
        {
            textColor = Color.white;
            textMesh.SetText("BLOCK!");
        }
        else
        {
            textMesh.SetText(damageAmount.ToString());
            if (isCriticalHit)
            {
                textMesh.fontSize = 4;
                ColorUtility.TryParseHtmlString("#FF2B00", out textColor);
            }
            else
            {
                textMesh.fontSize = 3;
                ColorUtility.TryParseHtmlString("#FFC500", out textColor);
            }
        }

        textMesh.color = textColor;
        disapperTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(0.7f, 1) * 4f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= 8f * Time.deltaTime * moveVector;

        if (disapperTimer > DISAPPEAR_TIMER_MAX / 2)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += increaseScaleAmount * Time.deltaTime * Vector3.one;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= decreaseScaleAmount * Time.deltaTime * Vector3.one;
        }
        disapperTimer -= Time.deltaTime;
        if(disapperTimer < 0)
        {
            float disapperSpeed = 3f;
            textColor.a -= disapperSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
