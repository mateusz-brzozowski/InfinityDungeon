using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class GoldStoreWindow : MonoBehaviour
{
    private EquipmentSystem equipmentSystem;
    private Text goldAmountText;
    [SerializeField] GameObject restorePurchaseButton;

    private void Awake()
    {
        DisableRestorePurchasebutton();
    }

    public void Setup()
    {
        goldAmountText = transform.Find("GoldBox/GoldAmountText").GetComponent<Text>();
    }

    private void SetGoldAmountText(int goldAmount)
    {
        goldAmountText.text = goldAmount.ToString();
    }

    public void SetEquipmentSystem(EquipmentSystem equipmentSystem)
    {
        this.equipmentSystem = equipmentSystem;
        equipmentSystem.OnGoldAmountChanged += GoldStoreWindow_OnGoldAmountChanged;
        SetGoldAmountText(equipmentSystem.GetGoldAmount());
    }

    private void GoldStoreWindow_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        SetGoldAmountText(equipmentSystem.GetGoldAmount());
    }

    private string gold100 = "com.infinityjourney.100.gold";
    private string gold500 = "com.infinityjourney.500.gold";
    private string gold1000 = "com.infinityjourney.1000.gold";

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == gold100)
        {
            //reward the player 100 Gold
            Debug.Log("Give 1000 Gold");
            equipmentSystem.AddGoldAmount(1000);
        }
        if (product.definition.id == gold500)
        {
            //reward the player 500 Gold
            Debug.Log("Give 5000 Gold");
            equipmentSystem.AddGoldAmount(5000);
        }
        if (product.definition.id == gold1000)
        {
            //reward the player 1000 Gold
            Debug.Log("Give 10000 Gold");
            equipmentSystem.AddGoldAmount(10000);
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Purchse of " + product.definition.id + " failed due to " + reason);
    }

    private void DisableRestorePurchasebutton()
    {
        if(Application.platform != RuntimePlatform.IPhonePlayer)
        {
            restorePurchaseButton.SetActive(false);
        }
    }
}
