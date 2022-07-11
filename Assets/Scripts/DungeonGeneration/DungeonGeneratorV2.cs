using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorV2 : MonoBehaviour
{
    [Header("Settings")]
    public DungeonSettings settings;

    [SerializeField] private float SpawnChamberSpawnChance;
    private int roomStepLimit { get { return GetDistance(entryChamber.RelativeRoomPosition, exitChamber.RelativeRoomPosition) + settings.MaxSubChamberDetours; } }
    private Chamber entryChamber;
    private Chamber exitChamber;
    private Vector2Int roomSize;

    public DungeonData dungeonData { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if(settings.seed != 0)
        {
            Random.InitState(settings.seed);
        }
        roomSize = new Vector2Int(settings.RoomWidth, settings.RoomHeight);
        GenerateDungeon();
        GetComponent<DungeonBuilder>().BuildDungeon(dungeonData);
    }
    public void GenerateDungeon()
    {
        dungeonData = new DungeonData();
        CreateCoreRooms(dungeonData);
        ChainCoreRooms(dungeonData);
        CreateSubRooms(dungeonData);
        BuildChamberTiles(dungeonData);
        CreateCorridors(dungeonData);
    }


    private void CreateCoreRooms(DungeonData dungeonData)
    {
        Vector2Int entryRelativePosition = new Vector2Int(Random.Range(0, settings.MaxDungeonWidth), 0);
        Vector2Int entryPosition = new Vector2Int(entryRelativePosition.x * (roomSize.x + settings.RoomDistance), 0);
        entryChamber = new Chamber(roomSize, entryPosition, entryRelativePosition);
        dungeonData.chambers.Add(entryChamber.RelativeRoomPosition, entryChamber);

        Vector2Int exitRelativePosition = new Vector2Int(Random.Range(0, settings.MaxDungeonWidth), settings.MaxDungeonHeight - 1);
        Vector2Int exitPosition = new Vector2Int(exitRelativePosition.x * (roomSize.x + settings.RoomDistance), exitRelativePosition.y * (roomSize.y + settings.RoomDistance));
        exitChamber = new Chamber(roomSize, exitPosition, exitRelativePosition);
        dungeonData.chambers.Add(exitChamber.RelativeRoomPosition, exitChamber);
    }
    private void ChainCoreRooms(DungeonData dungeonData)
    {
        int step = 0;
        Chamber current = entryChamber;
        List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        while (current != exitChamber)
        {
            Vector2Int roomTarget = directions[Random.Range(0, directions.Count)];
            Vector2Int relativeRoomPos = current.RelativeRoomPosition + roomTarget;
            if (GetDistance(relativeRoomPos, exitChamber.RelativeRoomPosition) > roomStepLimit - step || relativeRoomPos.x < 0 || relativeRoomPos.x >= settings.MaxDungeonWidth || relativeRoomPos.y < 0 || relativeRoomPos.y >= settings.MaxDungeonHeight)
            {
                directions.Remove(roomTarget);
                if (directions.Count == 0)
                {
                    Debug.Log("Couldn't find a path from available directions!");
                    return;
                }
            }
            else
            {
                step++;
                if (relativeRoomPos == exitChamber.RelativeRoomPosition)
                {
                    current.connectedChambers.Add(exitChamber);
                    return;
                }else if (dungeonData.chambers.ContainsKey(relativeRoomPos))
                {
                    Chamber existingChamber = dungeonData.chambers[relativeRoomPos];
                    if (!existingChamber.connectedChambers.Contains(current))
                    {
                        existingChamber.connectedChambers.Add(current);
                    }
                    current = existingChamber;
                }
                else
                {
                    Vector2Int roomPosition = new Vector2Int(current.Position.x + roomTarget.x * (roomSize.x + settings.RoomDistance), current.Position.y + roomTarget.y * (roomSize.y + settings.RoomDistance));
                    Chamber newChamber = CreateChamber(roomSize, roomPosition, relativeRoomPos);
                    newChamber.connectedChambers.Add(current);
                    dungeonData.chambers.Add(relativeRoomPos, newChamber);
                    current = newChamber;
                    directions = new List<Vector2Int>() { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
                }
            }
        }
    }


    private void BuildChamberTiles(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int, Chamber> pair in dungeonData.chambers)
        {
            for (int x = 0; x < pair.Value.RoomDimensions.x; x++)
            {
                for (int y = 0; y < pair.Value.RoomDimensions.y; y++)
                {
                    Vector2Int tilePosition = new Vector2Int(pair.Value.Position.x + x, pair.Value.Position.y + y);
                    dungeonData.tiles.Add(tilePosition);
                    dungeonData.nodes.Add(tilePosition, new Node(tilePosition, null, 0, 0));
                }
            }
            if(pair.Value == entryChamber)
            {
                Vector2Int position = new Vector2Int(entryChamber.Position.x + Mathf.RoundToInt(entryChamber.RoomDimensions.x / 2),entryChamber.Position.y - 1);
                dungeonData.doors.Add(position);
                dungeonData.nodes.Add(position, new Node(position, null, 0, 0));
            }
            if (pair.Value == exitChamber)
            {
                Vector2Int position = new Vector2Int(exitChamber.Position.x + Mathf.RoundToInt(exitChamber.RoomDimensions.x / 2), exitChamber.Position.y + exitChamber.RoomDimensions.y);
                dungeonData.doors.Add(position);
                dungeonData.nodes.Add(position, new Node(position, null, 0, 0));
            }
        }
    }

 
    private void CreateSubRooms(DungeonData dungeonData)
    {
        for(int i = 0; i < settings.SubRoomIterations; i++)
        {
            List<Chamber> subRooms = new List<Chamber>();
            foreach (KeyValuePair<Vector2Int, Chamber> pair in dungeonData.chambers)
            {
                Chamber parent = pair.Value;
                Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.down, Vector2Int.up };
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int targetPosition = direction + parent.RelativeRoomPosition;
                    if (targetPosition.x < 0 || targetPosition.x >= settings.MaxDungeonWidth || targetPosition.y < 0 || targetPosition.y >= settings.MaxDungeonHeight)
                    {
                        continue;
                    }
                    if (Random.Range(0, 100) <= settings.RoomSpawnChance)
                    {
                        if (!dungeonData.chambers.ContainsKey(targetPosition))
                        {
                            Vector2Int roomPosition = new Vector2Int(pair.Value.Position.x + (roomSize.x + settings.RoomDistance) * direction.x, pair.Value.Position.y + (roomSize.y + settings.RoomDistance) * direction.y);
                            Vector2Int newDimensions = new Vector2Int(settings.RoomWidth, settings.RoomHeight);
                            Chamber subRoom = CreateChamber(roomSize, roomPosition, targetPosition);
                            parent.connectedChambers.Add(subRoom);
                            subRooms.Add(subRoom);
                        }
                        else
                        {
                            parent.connectedChambers.Add(dungeonData.chambers[targetPosition]);
                        }
                    }
                }
            }
            foreach (Chamber c in subRooms)
            {
                if (!dungeonData.chambers.ContainsKey(c.RelativeRoomPosition))
                {
                    dungeonData.chambers.Add(c.RelativeRoomPosition, c);
                }
            }
        }
    }
    private void CreateCorridors(DungeonData dungeonData)
    {
        foreach(KeyValuePair<Vector2Int, Chamber> pair in dungeonData.chambers)
        {
            Chamber chamber = pair.Value;
            foreach (Chamber neighbour in chamber.connectedChambers)
            {
                //Vector2Int perceivedCenter = new Vector2Int(Mathf.RoundToInt(chamber.RelativeRoomPosition.x * (settings.RoomWidth + settings.RoomDistance) * 0.5f), Mathf.RoundToInt(chamber.RelativeRoomPosition.y * (settings.RoomHeight + settings.RoomDistance) * 0.5f));
                for (int rw = 1; rw < settings.RoomDistance + 1; rw++)
                {
                    for(int w = 1; w <= settings.CorridorWidth; w++)
                    {
                        Vector2Int position = Vector2Int.zero;
                        if (chamber.RelativeRoomPosition.x > neighbour.RelativeRoomPosition.x)
                        {
                            position = new Vector2Int(chamber.RelativeRoomPosition.x * (settings.RoomWidth + settings.RoomDistance) - rw, chamber.RelativeRoomPosition.y * (settings.RoomHeight + settings.RoomDistance) + Mathf.RoundToInt(settings.RoomHeight / 2) - Mathf.FloorToInt(settings.CorridorWidth/2) + (w - 1));
                        }
                        if (chamber.RelativeRoomPosition.x < neighbour.RelativeRoomPosition.x)
                        {
                            position = new Vector2Int(chamber.RelativeRoomPosition.x * (settings.RoomWidth + settings.RoomDistance) + settings.RoomWidth + (rw - 1), chamber.RelativeRoomPosition.y * (settings.RoomHeight + settings.RoomDistance) + Mathf.RoundToInt(settings.RoomHeight / 2) - Mathf.FloorToInt(settings.CorridorWidth / 2) + (w - 1));
                        }
                        if (chamber.RelativeRoomPosition.y > neighbour.RelativeRoomPosition.y)
                        {
                            position = new Vector2Int(chamber.RelativeRoomPosition.x * (settings.RoomWidth + settings.RoomDistance) + Mathf.RoundToInt(settings.RoomWidth / 2) - Mathf.FloorToInt(settings.CorridorWidth/2) + (w - 1), chamber.RelativeRoomPosition.y * (settings.RoomHeight + settings.RoomDistance) - rw);
                        }
                        if (chamber.RelativeRoomPosition.y < neighbour.RelativeRoomPosition.y)
                        {
                            position = new Vector2Int(chamber.RelativeRoomPosition.x * (settings.RoomWidth + settings.RoomDistance) + Mathf.RoundToInt(settings.RoomWidth / 2) - Mathf.FloorToInt(settings.CorridorWidth / 2) + (w - 1), chamber.RelativeRoomPosition.y * (settings.RoomHeight + settings.RoomDistance) + settings.RoomHeight + (rw - 1));
                        }
                        if (!dungeonData.corridors.Contains(position))
                        {
                            dungeonData.corridors.Add(position);
                            dungeonData.nodes.Add(position, new Node(position, null, 0, 0));
                        }
                    }
                }
            }
        }
    }

    private Chamber CreateChamber(Vector2Int dimensions, Vector2Int position, Vector2Int relativePosition)
    {
        return Random.Range(0, 101) <= SpawnChamberSpawnChance ? new SpawnChamber(dimensions, position, relativePosition) : new Chamber(dimensions, position, relativePosition);
    }

    private int GetDistance(Vector2Int from, Vector2Int to)
    {
        Vector2Int direction = to - from;
        return Mathf.Abs(direction.x) + Mathf.Abs(direction.y);
    }
}
