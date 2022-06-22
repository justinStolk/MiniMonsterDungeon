using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Effect", menuName = "Mini Monster Dungeon/Equipment Effects/Armor Effect")]
public class ArmorEffect : EquipmentEffect
{
    [SerializeField] private int amount = 1;
    public override void ApplyEffect(BaseUnit target)
    {
        target.Armor += amount;
    }

    public override void RemoveEffect(BaseUnit target)
    {
        target.Armor -= amount;
    }
}
