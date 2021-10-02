using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem
{
    public event EventHandler OnGoldAmountChanged;

    private int goldAmount;

    public void AddGoldAmount(int goldAmount)
    {
        this.goldAmount += goldAmount;
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetGoldAmount(int goldAmount)
    {
        this.goldAmount = goldAmount;
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public bool TrySpendGoldAmount(int spendGoldAmount)
    {
        if (GetGoldAmount() >= spendGoldAmount)
        {
            goldAmount -= spendGoldAmount;
            OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }
}
