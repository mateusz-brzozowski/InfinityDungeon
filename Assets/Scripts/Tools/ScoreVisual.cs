using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI    ;

public class ScoreVisual : MonoBehaviour
{
    ScoreSystem scoreSystem;
    private Text currentScoreAmountText;
    private Text highScoreAmountText;

    private void Awake()
    {
        currentScoreAmountText = transform.Find("CurrentScoreAmountText").GetComponent<Text>();
        highScoreAmountText = transform.Find("HighScoreAmountText").GetComponent<Text>();
    }

    private void SetcurrentScoreAmountText(int currentScoreAmount)
    {
        currentScoreAmountText.text = currentScoreAmount.ToString();
    }

    private void SetHighScoreAmountText(int highScoreAmount)
    {
        highScoreAmountText.text = highScoreAmount.ToString();
    }

    public void SetScoreSystem(ScoreSystem scoreSystem)
    {
        this.scoreSystem = scoreSystem;

        SetHighScoreAmountText(scoreSystem.GetHighScoreAmount());
        SetcurrentScoreAmountText(scoreSystem.GetCurrentScoreAmount());

        scoreSystem.OnHighScoreChanged += ScoreSystem_OnHighScoreChanged;
        scoreSystem.OnCurrentScoreChanged += ScoreSystem_OnCurrentScoreChanged;
    }

    private void ScoreSystem_OnHighScoreChanged(object sender, System.EventArgs e)
    {
        // High Score Changed, update high score text
        SetHighScoreAmountText(scoreSystem.GetHighScoreAmount());
    }

    private void ScoreSystem_OnCurrentScoreChanged(object sender, System.EventArgs e)
    {
        // Current Score Changed, update current score text
        SetcurrentScoreAmountText(scoreSystem.GetCurrentScoreAmount());
    }
}
