using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentVisual : MonoBehaviour
{
    EquipmentSystem equipmentSystem;
    private Text goldAmountText;

    private void Awake()
    {
        goldAmountText = transform.Find("GoldAmountText").GetComponent<Text>();
    }

    private void SetGoldAmountText(int goldAmount)
    {
        goldAmountText.text = goldAmount.ToString();
    }

    public void SetEquipmentSystem(EquipmentSystem equipmentSystem)
    {
        this.equipmentSystem = equipmentSystem;

        SetGoldAmountText(equipmentSystem.GetGoldAmount());

        equipmentSystem.OnGoldAmountChanged += EquipmentSystem_OnGoldAmountChanged;
    }

    private void EquipmentSystem_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        // Gold amount changed, update gold text
        SetGoldAmountText(equipmentSystem.GetGoldAmount());
    }
}
