using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem
{
    public event EventHandler OnHighScoreChanged;
    public event EventHandler OnCurrentScoreChanged;

    private int currentScoreAmount;
    private int highScoreAmount;

    public void AddCurrentScoreAmount(int currentScoreAmount)
    {
        this.currentScoreAmount += currentScoreAmount;
        OnCurrentScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentScoreAmount()
    {
        return currentScoreAmount;
    }

    public void SetHighScoreAmount(int highScoreAmount)
    {
        this.highScoreAmount = highScoreAmount;
        OnHighScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetHighScoreAmount()
    {
        return highScoreAmount;
    }

    public bool TrySetHighScore()
    {
        if (GetCurrentScoreAmount() > GetHighScoreAmount())
        {
            SetHighScoreAmount(GetCurrentScoreAmount());
            return true;
        }
        else
        {
            return false;
        }
    }
}
