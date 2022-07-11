using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : BaseUnit
{
    public Vector2Int Position { get; private set; }
    public List<Enemy> EnemiesInRange { get; private set; }

    [SerializeField] private float moveSpeed = 5;

    private AStar aStar;

    private bool hasMoved = false;

    private Vector2Int previousPosition;

    private List<Vector2Int> activeMoveRangeTiles;
    private List<Vector2Int> activeAttackRangeTiles;
    private List<Vector2Int> path;

    private void Start()
    {
        aStar = new AStar(DungeonData.PublicData.nodes, false);
        Position = Vector3ToVector2Int(transform.position);

        EventSystem.Subscribe(EventType.ON_PLAYER_ATTACK, AttackEnemies);
    }
    // Update is called once per frame
    void Update()
    {
        if (path != null && path.Count > 0)
        {
            if (transform.position != Vector2IntToVector3(path[0]))
            {
                transform.position = Vector3.MoveTowards(transform.position, Vector2IntToVector3(path[0]), moveSpeed * Time.deltaTime);
            }
            else
            {
                path.RemoveAt(0);
                if(path.Count == 0)
                {
                    EventSystem.CallEvent(EventType.ON_PLAYER_MOVED);
                    List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
                    bool enemyInRange = false;
                    EnemiesInRange = new();
                    activeAttackRangeTiles = new();
                    foreach (Vector2Int dir in directions)
                    {
                        Vector2Int target = Position + dir;
                        if (DungeonData.PublicData.nodes.ContainsKey(target))
                        {
                            Node evaluatedNeighbourNode = DungeonData.PublicData.nodes[target];
                            if(evaluatedNeighbourNode.occupyingElement != null)
                            {
                                Enemy closeEnemy = evaluatedNeighbourNode.occupyingElement.GetComponent<Enemy>();
                                if(closeEnemy != null)
                                {
                                    Debug.Log($"{closeEnemy.name} is occupying the node at {target} and can be attacked!");
                                    DungeonData.PublicData.attackRangeTiles[target].SetActive(true);
                                    activeAttackRangeTiles.Add(target);
                                    enemyInRange = true;
                                    EnemiesInRange.Add(closeEnemy);
                                }
                            }

                        }
                    }
                    if (enemyInRange)
                    {
                        EventSystem.CallEvent(EventType.ON_ENEMY_IN_RANGE);
                    }
                    //Show interface to confirm the movement, or not
                }
            }
        }
    }

    //private Task MoveToTile()
    //{
    //    if (path != null && path.Count > 0)
    //    {
    //        if (transform.position != Vector2IntToVector3(path[0]))
    //        {
    //            transform.position = Vector3.MoveTowards(transform.position, Vector2IntToVector3(path[0]), moveSpeed * Time.deltaTime);
    //        }
    //        else
    //        {
    //            path.RemoveAt(0);
    //        }
    //    }
    //    return Task.CompletedTask;
    //}

    public void Unfreeze()
    {
        hasMoved = false;
    }

    public bool OnSelected()
    {
        if (hasMoved)
        {
            return false;
        }

        activeMoveRangeTiles = aStar.GetNodesInRange(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)), MoveRange);
        foreach(Vector2Int v in activeMoveRangeTiles)
        {
            DungeonData.PublicData.moveRangeTiles[v].SetActive(true);
        }
        return true;
    }

    public void OnDeselected()
    {
        foreach(Vector2Int v in activeMoveRangeTiles)
        {
            DungeonData.PublicData.moveRangeTiles[v].SetActive(false);
        }
        if(activeAttackRangeTiles != null)
        {
            foreach (Vector2Int a in activeAttackRangeTiles)
            {
                DungeonData.PublicData.attackRangeTiles[a].SetActive(false);
            }
        }
    }

    public void MoveToTile(Vector2Int tilePosition)
    {
        if (activeMoveRangeTiles.Contains(tilePosition))
        {
            previousPosition = Position;
            Position = tilePosition;
            path = aStar.FindPathToTarget(Vector3ToVector2Int(transform.position), tilePosition);
            DungeonData.PublicData.nodes[previousPosition].occupyingElement = null;
            DungeonData.PublicData.nodes[Position].occupyingElement = this.gameObject;
            hasMoved = true;
        }
        OnDeselected();
    }

    public void AttackEnemies()
    {
        foreach(Enemy e in EnemiesInRange)
        {
            e.TakeAttack(Attack);
        }
        OnDeselected();
    }

    protected override void Die()
    {
        EventSystem.CallEvent(EventType.ON_PLAYER_DIE);
    }

    private Vector2Int Vector3ToVector2Int(Vector3 from)
    {
        return new Vector2Int(Mathf.RoundToInt(from.x), Mathf.RoundToInt(from.y));
    }

    private Vector3 Vector2IntToVector3(Vector2Int from)
    {
        return new Vector3(from.x, from.y, 0);
    }
}
