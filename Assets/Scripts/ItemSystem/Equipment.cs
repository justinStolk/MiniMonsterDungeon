using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Mini Monster Dungeon/Items/Equipment")]
public class Equipment : BaseItem
{
    [SerializeField] private EquipmentEffect[] equipmentEffects;
    public void OnEquip(BaseUnit target)
    {
        foreach(EquipmentEffect effect in equipmentEffects)
        {
            effect.ApplyEffect(target);
        }
    }

    public void OnUnequip(BaseUnit target)
    {
        foreach (EquipmentEffect effect in equipmentEffects)
        {
            effect.RemoveEffect(target);
        }
    }
 }
