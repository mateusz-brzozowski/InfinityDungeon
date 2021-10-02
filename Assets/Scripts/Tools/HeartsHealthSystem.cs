using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsHealthSystem
{
    public const int MAX_FRAGMENT_AMOUNT = 4;

    public event EventHandler OnDamged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private List<Heart> heartList;
    public HeartsHealthSystem(int heartAmount)
    {
        heartList = new List<Heart>();
        for(int i =0; i < heartAmount; i++)
        {
            Heart heart = new Heart(4);
            heartList.Add(heart);
        }
    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public void Damage(int damageAmount)
    {
        // Cycle through all hearts starting from the end
        for (int i = heartList.Count -1; i >= 0; i--)
        {
            Heart heart = heartList[i];
            // Test if this heart can absorb damageAmount
            if(damageAmount >= heart.GetFragmentAmount())
            {
                // heart cannot absorb full damageAmount, damage heart and keep going to next heart
                damageAmount -= heart.GetFragmentAmount();
                heart.Damage(heart.GetFragmentAmount());
            }
            else
            {
                // heart can absorb full damage amount, absorb and break out of the cycle
                heart.Damage(damageAmount);
                break;
            }
        }

        OnDamged?.Invoke(this, EventArgs.Empty);

        if (IsDead())
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            Heart heart = heartList[i];
            int missingFragmnets = MAX_FRAGMENT_AMOUNT - heart.GetFragmentAmount();
            if(healAmount > missingFragmnets)
            {
                healAmount -= missingFragmnets;
                heart.Heal(missingFragmnets);
            }
            else
            {
                heart.Heal(healAmount);
                break;
            }
        }
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return heartList[0].GetFragmentAmount() == 0;
    }

    // Represents a Single Heart
    public class Heart
    {
        private int fragments;

        public Heart(int fragments)
        {
            this.fragments = fragments;
        }

        public int GetFragmentAmount()
        {
            return fragments;
        }

        public void SetFragments(int fragments)
        {
            this.fragments = fragments;
        }

        public void Damage(int damageAmount)
        {
            if (damageAmount >= fragments)
            {
                fragments = 0;
            }
            else
            {
                fragments -= damageAmount;
            }
        }

        public void Heal(int healAmount)
        {
            if(fragments + healAmount > MAX_FRAGMENT_AMOUNT)
            {
                fragments = MAX_FRAGMENT_AMOUNT;
            }
            else
            {
                fragments += healAmount;
            }
        }

    }
}
