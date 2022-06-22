using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Effect", menuName = "Mini Monster Dungeon/Equipment Effects/Attack Effect")]
public class AttackEffect : EquipmentEffect
{
    [SerializeField] private int amount = 1;
    public override void ApplyEffect(BaseUnit target)
    {
        target.Attack += amount;
    }

    public override void RemoveEffect(BaseUnit target)
    {
        target.Attack -= amount;
    }

}
