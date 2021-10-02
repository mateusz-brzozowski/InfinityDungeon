using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EquipmentScriptableObject", menuName = "ScriptableObjects/EquipmentScriptableObject")]
public class EquipmentScriptableObject : ItemScriptableObject
{
    public EquipmentSlot equipSlot = EquipmentSlot.Weapon;

    public int armorModifier = 0;
    public int damageModifier = 0;
    public float rangeModifier = 1;
    public float thrust = 3;

    public int requiredLevel = 1;
    public int requiredGold = 5;
}

public enum EquipmentSlot {Head, Necklace, Chest, Legs, Weapon, Shield, Feet, Backpack, Ring, Arrows};