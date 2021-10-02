 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterScriptableObject", fileName = "New CharacterScriptableObject")]
public class CharacterScriptableObject : ScriptableObject
{
    public new string name = "New Item";

    public Sprite icon = null;
    public Sprite[] runSprites = null;
    public GameObject onMoveEffect = null;
    public GameObject dieEffect = null;

    public int maxhealth = 1;
    public int armorModifier = 0;
    public int damageModifier = 0;

    public int requiredLevel = 1;
    public int requiredGold = 0;
}
