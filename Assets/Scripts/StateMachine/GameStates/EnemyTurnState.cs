using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyTurnState : State<GameManager>
{
    private List<Enemy> allEnemies;
    public EnemyTurnState(GameManager owner) : base(owner)
    {
        Owner = owner;
    }

    public override void OnEnter()
    {
        allEnemies = Object.FindObjectsOfType<Enemy>().ToList();
        foreach(Enemy e in allEnemies)
        {
            e.Unfreeze();
        }
    }

    public override void OnUpdate()
    {
        HandleEnemyActions();
    }

    public override void OnExit()
    {

    }

    public bool AllEnemiesFinishedTurn()
    {
        return allEnemies.Count == 0;
    }

    private void HandleEnemyActions()
    {
        if(allEnemies.Count == 0)
        {
            return;
        }

        Enemy activeEnemy = allEnemies[0];
        if (!activeEnemy.IsMoving)
        {
            activeEnemy.GetPossibleDestinations();
            activeEnemy.MoveToDesiredDestination();
        }
        if (activeEnemy.ArrivedAtDestination())
        {
            allEnemies.Remove(allEnemies[0]);
            return;
        }
    }
}
