using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWindow : MonoBehaviour
{ 
    public event EventHandler OnCurrentCharacterChanged;
    private LevelSystem levelSystem;
    private EquipmentSystem equipmentSystem;

    private CharacterScriptableObject currentCharacter;
    private int characterIndex;
    private CharacterScriptableObjectAtlas allCharacters, ownCharacters;

    private Text goldAmountText, levelAmountText, healthText, damageText, armorText, LevelText, GoldText;
    private Image characterSprite;
    private GameObject currentCharacterCheck;
    private GameObject purchaseButton, selectButton;

    private AudioClip acceptSoundEffect, declineSoundEffect;
    private AudioSource audioSource;

    public void Setup()
    {
        goldAmountText = transform.Find("CharacterSystem/RequirementsValues/GoldAmountText").GetComponent<Text>();
        levelAmountText = transform.Find("CharacterSystem/RequirementsValues/LevelAmountText").GetComponent<Text>();

        healthText = transform.Find("CharacterSystem/Stats/Health/Text").GetComponent<Text>();
        damageText = transform.Find("CharacterSystem/Stats/Damage/Text").GetComponent<Text>();
        armorText = transform.Find("CharacterSystem/Stats/Armor/Text").GetComponent<Text>();
        LevelText = transform.Find("CharacterSystem/Requirements/Level/Text").GetComponent<Text>();
        GoldText = transform.Find("CharacterSystem/Requirements/Gold/Text").GetComponent<Text>();
        characterSprite = transform.Find("CharacterSystem/Character").GetComponent<Image>();
        currentCharacterCheck = transform.Find("CharacterSystem/CurrentCharacterCheck").gameObject;

        purchaseButton = transform.Find("PurchaseButton").gameObject;
        selectButton = transform.Find("SelectButton").gameObject;

        audioSource = transform.Find("SoundSource").GetComponent<AudioSource>();
        acceptSoundEffect = Resources.Load<AudioClip>("SoundEffects/zapsplat_multimedia_button_click_007_53868");
        declineSoundEffect = Resources.Load<AudioClip>("SoundEffects/zapsplat_multimedia_button_click_002_53863");

        //SetGoldAmountText(equipmentSystem.GetGoldAmount());
        //SetLevelAmountText(levelSystem.GetLevelNumber());
        //characterIndex = 0;
        //SetCharacter(allCharacters.GetCharacterAtIndex(characterIndex));

    }

    public CharacterScriptableObject GetCurrentCharacter()
    {
        return currentCharacter;
    }

    private void SetCharacter(CharacterScriptableObject currentCharacter)
    {
        healthText.text = (currentCharacter.maxhealth * 4).ToString();
        damageText.text = currentCharacter.damageModifier.ToString();
        armorText.text = currentCharacter.armorModifier.ToString();
        LevelText.text = currentCharacter.requiredLevel.ToString();
        GoldText.text = currentCharacter.requiredGold.ToString();
        if (!ownCharacters.IsInList(currentCharacter))
        {
            purchaseButton.SetActive(true);
            selectButton.SetActive(false);
            characterSprite.color = Color.black;
        }
        else
        {
            purchaseButton.SetActive(false);
            selectButton.SetActive(true);
            characterSprite.color = Color.white;

        }
        Debug.Log(this.currentCharacter + ", " + currentCharacter);
        if (this.currentCharacter == currentCharacter)
            currentCharacterCheck.SetActive(true);
        else
            currentCharacterCheck.SetActive(false);
        characterSprite.sprite = currentCharacter.icon;

        //this.currentCharacter = currentCharacter;
    }

    public void SetCharacterScriptableObject(
        CharacterScriptableObjectAtlas allCharacters, CharacterScriptableObjectAtlas ownCharacters, CharacterScriptableObject currentCharacter)
    {
        this.allCharacters = allCharacters;
        this.ownCharacters = ownCharacters;
        this.currentCharacter = currentCharacter;
        characterIndex = allCharacters.GetIndex(currentCharacter);
        SetCharacter(currentCharacter);
    }


    private void SetLevelAmountText(int levelAmount)
    {
        levelAmountText.text = (levelAmount + 1).ToString();
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        levelSystem.OnLevelChanged += CharacterWindow_OnLevelAmountChanged;
        SetLevelAmountText(levelSystem.GetLevelNumber());
    }

    private void CharacterWindow_OnLevelAmountChanged(object sender, System.EventArgs e)
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
        equipmentSystem.OnGoldAmountChanged += CharacterWindow_OnGoldAmountChanged;
        SetGoldAmountText(equipmentSystem.GetGoldAmount());
    }

    private void CharacterWindow_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        SetGoldAmountText(equipmentSystem.GetGoldAmount());
    }

    public void OnPurchase()
    {
        CharacterScriptableObject characterOnIndex = allCharacters.GetCharacterAtIndex(characterIndex);
        if (levelSystem.GetLevelNumber() + 1 >= characterOnIndex.requiredLevel && !ownCharacters.IsInList(characterOnIndex))
        {
            if (equipmentSystem.TrySpendGoldAmount(characterOnIndex.requiredGold))
            {
                audioSource.PlayOneShot(acceptSoundEffect);
                ownCharacters.AddCharacter(characterOnIndex);
                SetCharacter(characterOnIndex);
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
        CharacterScriptableObject characterOnIndex = allCharacters.GetCharacterAtIndex(characterIndex);
        if (ownCharacters.IsInList(characterOnIndex))
        {
            currentCharacter = characterOnIndex;
            SetCharacter(characterOnIndex);
            OnCurrentCharacterChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public void OnLeftButton()
    {
        characterIndex--;
        if (characterIndex < 0)
            characterIndex = allCharacters.GetNumberOfCharacters() - 1;
        SetCharacter(allCharacters.GetCharacterAtIndex(characterIndex));
    }
    public void OnRightButton()
    {
        characterIndex++;
        if (characterIndex >= allCharacters.GetNumberOfCharacters())
            characterIndex = 0;
        SetCharacter(allCharacters.GetCharacterAtIndex(characterIndex));
    }
}
