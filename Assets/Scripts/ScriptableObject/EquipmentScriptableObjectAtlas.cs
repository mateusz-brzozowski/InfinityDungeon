using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentScriptableObject Atlas", fileName = "New EquipmentScriptableObject Atlas")]
public class EquipmentScriptableObjectAtlas : ScriptableObject
{
    [SerializeField] EquipmentScriptableObject defaultEquipment = null;
    [SerializeField] List<EquipmentScriptableObject> equipments = new List<EquipmentScriptableObject>();

    public EquipmentScriptableObject GetEquipmentAtIndex(int index)
    {
        if(index == -1)
            return defaultEquipment;
        else if (index < equipments.Count && index >= 0)
            return equipments[index];
        else
        {
            Debug.LogWarning("index is not in range, add more skins or request a lower index");
            return defaultEquipment;
        }
    }

    public void AddEquipment(EquipmentScriptableObject equipmentScriptableObject)
    {
        equipments.Add(equipmentScriptableObject);
    }

    public int GetNumberOfEquipments()
    {
        return equipments.Count;
    }

    public bool IsInList(EquipmentScriptableObject equipmentScriptableObject)
    {
        foreach (EquipmentScriptableObject equipment in equipments)
        {
            if (equipment == equipmentScriptableObject)
            {
                return true;
            }
        }
        return false;
    }

    public List<EquipmentScriptableObject> GetList()
    {
        return equipments;
    }

    public void SetList(List<EquipmentScriptableObject> equipmentScriptableObjects)
    {
        equipments = equipmentScriptableObjects;
    }

    public int GetIndex(EquipmentScriptableObject equipmentScriptableObject)
    {
        return equipments.IndexOf(equipmentScriptableObject);
    }
}
