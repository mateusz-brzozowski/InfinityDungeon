using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    public event EventHandler OnInstantExperienceChanged;
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private static readonly int[] experiencePerLevel = new[] { 
        400, 800, 1400, 2200, 3400, 4900, 5000, 7500, 10000, 12500, 15000, 20000, 23000, 26000, 29000, 32000, 36000, 40000, 45000};

    private int level;
    private int experience;

    private int experienceAmount; 
    private string reason;

    public LevelSystem()
    {
        level = 0;
        experience = 0;
        experienceAmount = 0;
        reason = "";
    }

    public void AddExperience(int amount, string reason = "")
    {
        if (!IsMaxLevel())
        {
            experienceAmount = amount;
            this.reason = reason;
            experience += amount;
            OnInstantExperienceChanged?.Invoke(this, EventArgs.Empty);
            while (!IsMaxLevel() && experience >= GetExperienceToNextLevel(level))
            {
                // Enough experience to level up
                experience -= GetExperienceToNextLevel(level);
                level++;
                OnLevelChanged?.Invoke(this, EventArgs.Empty);
            }
            OnExperienceChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public float GetExperienceNormalized()
    {
        if (IsMaxLevel())
        {
            return 1f;
        }
        else
        {
            return (float)experience / GetExperienceToNextLevel(level);
        }
    }

    public int GetExperience()
    {
        return experience;
    }

    public int GetExperienceToNextLevel(int level)
    {
        if (level < experiencePerLevel.Length)
        {
            return experiencePerLevel[level];
        }
        else
        {
            // Level Invalid
            Debug.LogError("Level invalid: " + level);
            return 100;
        }
    }

    public bool IsMaxLevel()
    {
        return IsMaxLevel(level);
    }

    public bool IsMaxLevel(int level)
    {
        return level == experiencePerLevel.Length - 1;
    }

    public int GetExperienceAmount()
    {
        return experienceAmount;
    }

    public string GetReasonText()
    {
        return reason;
    }

    public void SetExperience(int experience)
    {
        this.experience = experience;
        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetLevel(int level)
    {
        this.level = level;
        OnLevelChanged?.Invoke(this, EventArgs.Empty);
    }
}
