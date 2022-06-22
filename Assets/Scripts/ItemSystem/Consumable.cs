using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Mini Monster Dungeon/Items/Consumable")]
public class Consumable : BaseItem
{
    [SerializeField] private ConsumableEffect[] consumableEffects;

    public void OnConsume(BaseUnit target)
    {
        foreach(ConsumableEffect effect in consumableEffects)
        {
            effect.ApplyEffect(target);
        }
    }

}
