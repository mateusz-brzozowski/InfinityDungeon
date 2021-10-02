using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon Create(Transform transform, EquipmentScriptableObject equipmentScriptableObject)
    {
        Transform weaponTransform = Instantiate(Resources.Load<Transform>("Prefabs/Weapon"), transform.position, Quaternion.identity);
        weaponTransform.SetParent(transform);
        Weapon weapon = weaponTransform.GetComponent<Weapon>();
        weapon.Setup(equipmentScriptableObject);
        return weapon;
    }

    private EquipmentScriptableObject equipmentScriptableObject;
    private SpriteRenderer spriteRenderer;
    private Animator weaponAnim;

    private void Setup(EquipmentScriptableObject equipmentScriptableObject)
    {
        this.equipmentScriptableObject = equipmentScriptableObject;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        weaponAnim = gameObject.GetComponent<Animator>();
        spriteRenderer.sprite = equipmentScriptableObject.icon;
    }

    #region Weapon Getters
    public int GetDamageModifier()
    {
        return equipmentScriptableObject.damageModifier;
    }
    public float GetRangeModifier()
    {
        return equipmentScriptableObject.rangeModifier;
    }
    public int GetArmorModifier()
    {
        return equipmentScriptableObject.armorModifier;
    }
    public float GetThrust()
    {
        return equipmentScriptableObject.thrust;
    }
    #endregion

    #region Weapon Events
    // TODO: play attack animation with handle 
    public void OnAttack()
    {
        weaponAnim.SetTrigger("attack");
    }
    #endregion

}
