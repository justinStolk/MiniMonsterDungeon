using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : BaseUnit
{
    public bool HasMoved { get; private set; }
    public bool IsMoving { get; private set; }

    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private int skillPointsOnDeath = 1;
   
    private AStar aStar;


    private Vector2Int position;

    private Vector2Int targetPosition;

    private List<Vector2Int> activeMoveRangeTiles;
    private List<Vector2Int> path;

    // Start is called before the first frame update
    void Start()
    {
        position = Vector3ToVector2Int(transform.position);
        aStar = new AStar(false);
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
                    //StartCoroutine("HandleTimeDelay");

                    //position = Vector3ToVector2Int(transform.position);
                    DungeonData.PublicData.nodes[position].occupyingElement = this.gameObject;
                    List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
                    foreach (Vector2Int dir in directions)
                    {
                        Vector2Int target = position + dir;
                        if (DungeonData.PublicData.nodes.ContainsKey(target))
                        {
                            if (target == GameManager.Instance.Player.Position)
                            {
                                Debug.Log("I found a player next to me!");
                                GameManager.Instance.Player.TakeAttack(Attack);
                                break;
                            }

                        }
                    }
                    HasMoved = true;

                }
            }
        }
    }
    public void Unfreeze()
    {
        HasMoved = false;
        IsMoving = false;
    }

    public void GetPossibleDestinations()
    {
        activeMoveRangeTiles = aStar.GetNodesInRange(position, MoveRange);

        //Vector2Int closestUnoccupiedByPlayer = aStar.FindClosestTo(position, GameManager.Instance.Player.Position);
        //List<Vector2Int> pathToPlayer = aStar.FindPathToTarget(position, closestUnoccupiedByPlayer);
        //IsMoving = true;
        //foreach (Vector2Int pathNode in pathToPlayer)
        //{
        //    if (activeMoveRangeTiles.Contains(pathNode))
        //    {
        //        targetPosition = pathNode;
        //        continue;
        //    }
        //    return;
        //}
        float distance = float.MaxValue;
        foreach (Vector2Int v in activeMoveRangeTiles)
        {
            DungeonData.PublicData.moveRangeTiles[v].SetActive(true);
            float calculatedDistance = Vector2Int.Distance(v, GameManager.Instance.Player.Position);
            if (calculatedDistance < distance)
            {
                distance = calculatedDistance;
                //Debug.Log(distance);
                targetPosition = v;
                //This should get us the node closest to the player that we can also move towards.
            }
        }
    }

    public void MoveToDesiredDestination()
    {
        path = aStar.FindPathToTarget(position, targetPosition); 
        DungeonData.PublicData.nodes[position].occupyingElement = null;
        position = targetPosition;
        foreach (Vector2Int v in activeMoveRangeTiles)
        {
            DungeonData.PublicData.moveRangeTiles[v].SetActive(false);
        }
        IsMoving = true;
    }
    public bool ArrivedAtDestination()
    {
        return path.Count == 0;
        //if(path.Count == 0)
        //{
        //    List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        //    foreach (Vector2Int dir in directions)
        //    {
        //        Vector2Int target = position + dir;
        //        if (DungeonData.PublicData.nodes.ContainsKey(target))
        //        {
        //            Node evaluatedNeighbourNode = DungeonData.PublicData.nodes[target];
        //            if (evaluatedNeighbourNode.occupyingElement.TryGetComponent(out Player player))
        //            {
        //                player.TakeAttack(Attack);
        //                break;
        //            }

        //        }
        //    }
        //    return true;
        //}
        //return false;
    }
    //private IEnumerator HandleTimeDelay()
    //{
    //    yield return new WaitForSeconds(2);
    //    HasMoved = true;
    //    IsMoving = false;

    //}

    protected override void Die()
    {
        GameManager.Instance.GetSkillPoints(skillPointsOnDeath);
        Destroy(gameObject);
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
