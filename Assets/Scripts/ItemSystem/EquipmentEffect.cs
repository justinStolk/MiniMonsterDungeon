using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentEffect : ScriptableObject
{
    public abstract void ApplyEffect(BaseUnit target);
    public abstract void RemoveEffect(BaseUnit target);

}
