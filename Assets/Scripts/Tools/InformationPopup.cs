using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPopup : MonoBehaviour
{
    public static InformationPopup Create(Transform transform, string information, Color color, int textSize = 40, float disappearTime = 0.5f)
    {
        Transform informationPopupTransform = Instantiate(Resources.Load<Transform>("Prefabs/InformationPopup"), transform.position, Quaternion.identity);
        informationPopupTransform.SetParent(transform);
        InformationPopup informationPopup = informationPopupTransform.GetComponent<InformationPopup>();
        informationPopup.Setup(information, textSize, color, disappearTime);
        return informationPopup;
    }

    private Text text;
    private Vector3 moveVector;

    private void Awake()
    {
        text = transform.GetComponent<Text>();
    }

    public void Setup(string information, int textSize, Color color, float disappearTime)
    {
        text.color = color;
        text.fontSize = textSize;
        text.text = information;
        moveVector = new Vector3(0f, 100f);
        StartCoroutine(FadeOut(disappearTime));
    }

    IEnumerator FadeOut(float disappearTime)
    {
        while (text.color.a > 0)
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 4f * Time.deltaTime;
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, -1), disappearTime * Time.deltaTime);
            //transform.localScale -= Vector3.one * (1/disappearTime) * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
