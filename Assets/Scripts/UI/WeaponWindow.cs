using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWindow : MonoBehaviour
{
    public event EventHandler OnCurrentWeaponChanged;
    private LevelSystem levelSystem;
    private EquipmentSystem equipmentSystem;

    private EquipmentScriptableObject currentWeapon;
    private int weaponIndex;
    private EquipmentScriptableObjectAtlas allWeapons, ownWeapons;

    private Text goldAmountText, levelAmountText, thrustText, damageText, armorText, levelText, goldText, nameText;
    private Image weaponSprite;
    private GameObject currentWeaponCheck;

    private GameObject purchaseButton, selectButton;

    private AudioClip acceptSoundEffect, declineSoundEffect;
    private AudioSource audioSource;

    public void Setup()
    {
        goldAmountText = transform.Find("WeaponSystem/RequirementsValues/GoldAmountText").GetComponent<Text>();
        levelAmountText = transform.Find("WeaponSystem/RequirementsValues/LevelAmountText").GetComponent<Text>();

        thrustText = transform.Find("WeaponSystem/Stats/Thrust/Text").GetComponent<Text>();
        damageText = transform.Find("WeaponSystem/Stats/Damage/Text").GetComponent<Text>();
        armorText = transform.Find("WeaponSystem/Stats/Armor/Text").GetComponent<Text>();
        levelText = transform.Find("WeaponSystem/Requirements/Level/Text").GetComponent<Text>();
        goldText = transform.Find("WeaponSystem/Requirements/Gold/Text").GetComponent<Text>();
        weaponSprite = transform.Find("WeaponSystem/Weapon").GetComponent<Image>();
        nameText = transform.Find("WeaponSystem/Stats/Name").GetComponent<Text>();
        currentWeaponCheck = transform.Find("WeaponSystem/currentWeaponCheck").gameObject;

        purchaseButton = transform.Find("PurchaseButton").gameObject;
        selectButton = transform.Find("SelectButton").gameObject;

        audioSource = transform.Find("SoundSource").GetComponent<AudioSource>();
        acceptSoundEffect = Resources.Load<AudioClip>("SoundEffects/zapsplat_multimedia_button_click_007_53868");
        declineSoundEffect = Resources.Load<AudioClip>("SoundEffects/zapsplat_multimedia_button_click_002_53863");

        //SetGoldAmountText(equipmentSystem.GetGoldAmount());
        //SetLevelAmountText(levelSystem.GetLevelNumber());
        //weaponIndex = 0;
        //SetWeapon(allWeapons.GetEquipmentAtIndex(weaponIndex));

    }

    public EquipmentScriptableObject GetCurrentWeapon()
    {
        return currentWeapon;
    }

    private void SetWeapon(EquipmentScriptableObject currentWeapon)
    {
        thrustText.text = currentWeapon.thrust.ToString();
        damageText.text = currentWeapon.damageModifier.ToString();
        armorText.text = currentWeapon.armorModifier.ToString();
        levelText.text = currentWeapon.requiredLevel.ToString();
        goldText.text = currentWeapon.requiredGold.ToString();
        nameText.text = currentWeapon.name.Replace(" ", "\n") + "\n";
        if (!ownWeapons.IsInList(currentWeapon))
        {
            purchaseButton.SetActive(true);
            selectButton.SetActive(false);
            weaponSprite.color = Color.black;
        }
        else
        {
            purchaseButton.SetActive(false);
            selectButton.SetActive(true);
            weaponSprite.color = Color.white;

        }
        if (this.currentWeapon == currentWeapon)
            currentWeaponCheck.SetActive(true);
        else
            currentWeaponCheck.SetActive(false);

        weaponSprite.sprite = currentWeapon.icon;
        weaponSprite.rectTransform.sizeDelta = new Vector2(currentWeapon.icon.rect.width * 15, currentWeapon.icon.rect.height * 15);

        //this.currentWeapon = currentWeapon;
    }

    public void SetCharacterScriptableObject(
        EquipmentScriptableObjectAtlas allWeapons, EquipmentScriptableObjectAtlas ownWeapons, EquipmentScriptableObject currentWeapon)
    {
        this.allWeapons = allWeapons;
        this.ownWeapons = ownWeapons;
        this.currentWeapon = currentWeapon;
        weaponIndex = allWeapons.GetIndex(currentWeapon);
        SetWeapon(currentWeapon);
    }


    private void SetLevelAmountText(int levelAmount)
    {
        levelAmountText.text = (levelAmount + 1).ToString();
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        levelSystem.OnLevelChanged += WeaponWindow_OnLevelAmountChanged;
        SetLevelAmountText(levelSystem.GetLevelNumber());
    }

    private void WeaponWindow_OnLevelAmountChanged(object sender, System.EventArgs e)
    {
        SetLevelAmountText(levelSystem.GetLevelNumber());
    }

    private void SetGoldAmountText(int goldAmount)
    {
        goldAmountText.text = goldAmount.ToString();
    }

    public void SetEquipmentSystem(EquipmentSystem equipmentSystem)
    {
        this.equipmentSystem = equipmentSystem;
        equipmentSystem.OnGoldAmountChanged += WeaponWindow_OnGoldAmountChanged;
        SetGoldAmountText(equipmentSystem.GetGoldAmount());
    }

    private void WeaponWindow_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        SetGoldAmountText(equipmentSystem.GetGoldAmount());
    }

    public void OnPurchase()
    {
        EquipmentScriptableObject weaponOnIndex = allWeapons.GetEquipmentAtIndex(weaponIndex);
        if (levelSystem.GetLevelNumber() + 1 >= weaponOnIndex.requiredLevel && !ownWeapons.IsInList(weaponOnIndex))
        {
            if (equipmentSystem.TrySpendGoldAmount(weaponOnIndex.requiredGold))
            {
                audioSource.PlayOneShot(acceptSoundEffect);
                ownWeapons.AddEquipment(weaponOnIndex);
                SetWeapon(weaponOnIndex);
                OnSelect();
            }
            else
                audioSource.PlayOneShot(declineSoundEffect);
        }
        else
            audioSource.PlayOneShot(declineSoundEffect);
    }
    public void OnSelect()
    {
        EquipmentScriptableObject weaponOnIndex = allWeapons.GetEquipmentAtIndex(weaponIndex);
        if (ownWeapons.IsInList(currentWeapon))
        {
            currentWeapon = weaponOnIndex;
            SetWeapon(weaponOnIndex);
            OnCurrentWeaponChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public void OnLeftButton()
    {
        weaponIndex--;
        if (weaponIndex < 0)
            weaponIndex = allWeapons.GetNumberOfEquipments() - 1;
        SetWeapon(allWeapons.GetEquipmentAtIndex(weaponIndex));
    }
    public void OnRightButton()
    {
        weaponIndex++;
        if (weaponIndex >= allWeapons.GetNumberOfEquipments())
            weaponIndex = 0;
        SetWeapon(allWeapons.GetEquipmentAtIndex(weaponIndex));
    }
}
