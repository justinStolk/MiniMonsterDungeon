using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    [TextArea(3,10)]
    [SerializeField] protected string _description;

    [SerializeField] protected Sprite _icon;
    public string Description { get { return _description; } }
    public Sprite Icon { get { return _icon; } }

}
