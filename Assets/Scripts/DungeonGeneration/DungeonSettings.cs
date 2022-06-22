using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Dungeon Settings", menuName = "Mini Monster Dungeon/Dungeon Settings")]
public class DungeonSettings : ScriptableObject
{
    [Range(0, 100)]
    public float RoomSpawnChance = 50f;
    [Min(1)]
    public int SubRoomIterations = 2;
    public int MinimumRoomWidth, MaximumRoomWidth, MinimumRoomHeight, MaximumRoomHeight;
    [Min(1)]
    public int MaxHorizontalRoomOffset, MaxVerticalRoomOffset = 1;
    public int RoomDistance = 3;
    public int CorridorWidth = 3;
    [Min(0)]
    public int MaxSubChamberDetours = 2;

    public int seed;
}
