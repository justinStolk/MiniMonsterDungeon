using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Mini Monster Dungeon/Consumable Effects/Damage Effect")]
public class DamageEffect : ConsumableEffect
{
    [Min(1)]
    [SerializeField] private int damageAmount = 1;

    public override void ApplyEffect(BaseUnit target)
    {
        target.TakeAttack(damageAmount);
    }
}
