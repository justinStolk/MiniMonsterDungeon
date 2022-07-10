using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnterState : State<GameManager>
{

    private bool isInitialized;
    public DungeonEnterState(GameManager owner) : base(owner)
    {
        Owner = owner;
    }

    public override void OnEnter()
    {
        Owner.Cursor = Object.Instantiate(Resources.Load("CursorPrefab") as GameObject);
        isInitialized = true;
    }


    public bool DungeonInitialized()
    {
        return isInitialized;
    }


}
