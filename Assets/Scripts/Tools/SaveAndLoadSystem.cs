using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadSystem
{
    public void Save(
        int goldAmount, int highScoreAmount, int level, int experience, 
        CharacterScriptableObject characterScriptableObject, EquipmentScriptableObject equipmentScriptableObject, 
        float musicVolume, float soundEffectsVolume, List<CharacterScriptableObject> characterScriptableObjectAtlas,
        List<EquipmentScriptableObject> equipmentScriptableObjectAtlas)
    {
        SaveObject saveObject = new SaveObject
        {
            goldAmount = goldAmount,
            highScoreAmount = highScoreAmount,
            level = level,
            experience = experience,
            currentCharacter = characterScriptableObject,
            currentWeapon = equipmentScriptableObject,
            musicVolume = musicVolume,
            soundEffectsVolume = soundEffectsVolume,
            ownCharacters = characterScriptableObjectAtlas,
            ownWeapons = equipmentScriptableObjectAtlas
        };

        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }

    public SaveObject Load()
    {
        string saveString = SaveSystem.Load();
        SaveObject saveObject;
        if (saveString != null)
        {
            // Save exist, load values form file
            saveObject = JsonUtility.FromJson<SaveObject>(saveString);

        }
        else
        {
            // No save, get default values
            saveObject = new SaveObject();
        }
        return saveObject;
    }

    public class SaveObject
    {
        public int goldAmount = 0;
        public int highScoreAmount = 0;
        public int level = 0;
        public int experience = 0;
        public CharacterScriptableObject currentCharacter;
        public EquipmentScriptableObject currentWeapon;
        public float musicVolume = 0;
        public float soundEffectsVolume = 0;
        public List<CharacterScriptableObject> ownCharacters = new List<CharacterScriptableObject>();
        public List<EquipmentScriptableObject> ownWeapons = new List<EquipmentScriptableObject>();
    }
}
