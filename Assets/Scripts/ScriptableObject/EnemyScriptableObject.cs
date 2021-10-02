using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyScriptableObject", fileName = "New EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    public new string name = "New EnemyScriptableObject";

    public Sprite icon = null;
    public Sprite[] runSprites = null;
    public GameObject dieEffect = null;

    public int maxhealth = 1;
    public int armorModifier = 0;
    public int damageModifier = 0;
    public int experience = 0;
    public int goldAmount = 1;
    public int scoreAmount = 1;
}
