using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Characters Atlas", fileName = "New Characters Atlas")]
public class CharacterScriptableObjectAtlas : ScriptableObject
{
    [SerializeField] CharacterScriptableObject defaultCharacter = null;
    [SerializeField] List<CharacterScriptableObject> characters = new List<CharacterScriptableObject>();

    public CharacterScriptableObject GetCharacterAtIndex(int index)
    {
        if (index < characters.Count && index >= 0)
            return characters[index];
        else
        {
            Debug.LogWarning("index is not in range, add more skins or request a lower index");
            return defaultCharacter;
        }
    }

    public void AddCharacter(CharacterScriptableObject characterScriptableObject)
    {
        characters.Add(characterScriptableObject);
    }

    public int GetNumberOfCharacters()
    {
        return characters.Count;
    }

    public bool IsInList(CharacterScriptableObject characterScriptableObject)
    {
        foreach (CharacterScriptableObject character in characters)
        {
            if (character == characterScriptableObject)
            {
                return true;
            }
        }
        return false;
    }

    public List<CharacterScriptableObject> GetList()
    {
        return characters;
    }

    public void SetList(List<CharacterScriptableObject> characterScriptableObjects)
    {
        characters = characterScriptableObjects;
    }

    public int GetIndex(CharacterScriptableObject characterScriptableObject)
    {
        return characters.IndexOf(characterScriptableObject);
    }
}
