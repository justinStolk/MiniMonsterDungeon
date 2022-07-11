using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EventType { 
    ON_PLAYER_TURN_STARTED, 
    ON_PLAYER_TURN_ENDED,
    ON_PLAYER_MOVED,
    ON_PLAYER_MOVE_CONFIRMED,
    ON_ENEMY_IN_RANGE,
    ON_PRIMED_FOR_ATTACK,
    ON_PLAYER_ATTACK,
    ON_PLAYER_DIE,
    ON_ACTION_EXECUTED,
    ON_ENEMY_TURN_STARTED,
    ON_ENEMY_TURN_ENDED,
    ON_DUNGEON_MENU_TOGGLED
}
public static class EventSystem
{
    private static Dictionary<EventType, Action> actions = new();
    public static void Subscribe(EventType eventType, Action actionToSubscribe)
    {
        if (!actions.ContainsKey(eventType))
        {
            actions.Add(eventType, null);
        }
        actions[eventType] += actionToSubscribe;
    }
    public static void Unsubscribe(EventType eventType, Action actionToUnsubscribe)
    {
        if (actions.ContainsKey(eventType))
        {
            actions[eventType] -= actionToUnsubscribe;
        }
    }
    public static void CallEvent(EventType eventType)
    {
        actions[eventType]?.Invoke();
    }


}
