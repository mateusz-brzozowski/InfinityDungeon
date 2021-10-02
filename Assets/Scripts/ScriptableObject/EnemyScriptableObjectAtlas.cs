using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemies Stats Atlas", fileName = "New Enemies Stats Atlas")]
public class EnemyScriptableObjectAtlas : ScriptableObject
{
    [SerializeField] EnemyScriptableObject defaultEnemy = null;
    [SerializeField] List<EnemyScriptableObject> enemies = new List<EnemyScriptableObject>();

    public EnemyScriptableObject GetCharacterAtIndex(int index)
    {
        if (index < enemies.Count && index >= 0)
            return enemies[index];
        else
        {
            Debug.LogWarning("index is not in range, add more skins or request a lower index");
            return defaultEnemy;
        }
    }

    public int GetNumberOfEnemies()
    {
        return enemies.Count;
    }
}
