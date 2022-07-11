using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Mini Monster Dungeon/Consumable Effects/Heal Effect")]
public class HealEffect : ConsumableEffect
{
    [Min(1)]
    [SerializeField] private int healAmount = 1;

    public override void ApplyEffect(BaseUnit target)
    {
        target.Heal(healAmount);
    }
}
