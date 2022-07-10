using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : BaseUnit
{

    [SerializeField] private float moveSpeed;

    private AStar aStar;

    private bool hasMoved = false;

    private Vector2Int position;
    private Vector2Int expectedPosition;

    private List<Vector2Int> activeMoveRangeTiles;
    private List<Vector2Int> path;

    private void Start()
    {
        aStar = new AStar(DungeonData.PublicData.nodes, false);
        position = Vector3ToVector2Int(transform.position);

        EventSystem.Subscribe(EventType.ON_PLAYER_MOVE_CONFIRMED, ConfirmMovement);

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

        Debug.Log("I should show nodes in range!");
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
    }

    public void MoveToTile(Vector2Int tilePosition)
    {
        if (activeMoveRangeTiles.Contains(tilePosition))
        {
            expectedPosition = tilePosition;
            path = aStar.FindPathToTarget(Vector3ToVector2Int(transform.position), tilePosition);            
        }
        OnDeselected();
    }

    public void ConfirmMovement()
    {
        DungeonData.PublicData.nodes[position].occupyingElement = null;
        position = expectedPosition;
        DungeonData.PublicData.nodes[position].occupyingElement = this.gameObject;
        hasMoved = true;
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
