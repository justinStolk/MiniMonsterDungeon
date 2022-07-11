using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExitState : State<GameManager>
{

    private bool exited;
    public DungeonExitState(GameManager owner) : base(owner)
    {
        Owner = owner;
    }

    public override void OnEnter()
    {
        Owner.PlayerDied = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("PreparationMenu");
        exited = true;
    }

    public override void OnExit()
    {
        exited = false;
    }
    public bool Exited()
    {
        return exited;
    }

}
