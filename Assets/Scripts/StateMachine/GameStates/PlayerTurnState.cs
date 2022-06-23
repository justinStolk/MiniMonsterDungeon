using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State<GameManager>
{
    public PlayerTurnState(GameManager owner) : base(owner)
    {
        Owner = owner;
    }
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

}
