using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Dungeon Settings", menuName = "Mini Monster Dungeon/Dungeon Settings")]
public class DungeonSettings : ScriptableObject
{
    [Range(0, 100)]
    public float RoomSpawnChance = 50f;
    [Min(0)]
    public int SubRoomIterations = 2;
    [Min(3)]
    public int RoomWidth, RoomHeight;
    [Min(1)]
    public int MaxDungeonWidth, MaxDungeonHeight = 1;
    public int RoomDistance = 3;
    public int CorridorWidth = 3;
    [Min(0)]
    public int MaxSubChamberDetours = 2;

    public int seed;
}
