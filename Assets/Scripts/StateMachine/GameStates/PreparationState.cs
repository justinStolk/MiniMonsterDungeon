using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationState : State<GameManager>
{

    public PreparationState(GameManager owner) : base(owner)
    {
        Owner = owner;
    }
    public override void OnEnter()
    {
        //Player enters preparation state
    }

    public override void OnExit()
    {
        //This is most likely right before a transition to the Player Turn State in the dungeon.
    }

    public override void OnUpdate()
    {
        //Player can upgrade their character, based on points gotten in previous runs.
    }
}
