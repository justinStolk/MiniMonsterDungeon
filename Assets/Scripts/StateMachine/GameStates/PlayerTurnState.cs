using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State<GameManager>
{
    private Player player;
    private GameObject cursor;
    private bool turnEnded;
    private Vector2Int cursorPosition;

    private bool playerSelected;

    public PlayerTurnState(GameManager owner) : base(owner)
    {
        Owner = owner;
        EventSystem.Subscribe(EventType.ON_PLAYER_TURN_ENDED, () => turnEnded = true);
    }

    public override void OnEnter()
    {
        turnEnded = false;
        if(cursor == null)
        {
            cursor = Owner.Cursor;
        }
        if(player == null)
        {
            player = Owner.Player;
        }
        Owner.OnConfirmed += OnClickCommand;
        Owner.OnMenuToggle += OnMenuToggle;
        player.Unfreeze();
    }

    public override void OnExit()
    {
        Owner.OnConfirmed -= OnClickCommand;
        Owner.OnMenuToggle -= OnMenuToggle;
    }

    public override void OnUpdate()
    {
        HandleCursorMovement();
    }

    private void HandleCursorMovement()
    {
        Vector3 worldMousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition = new Vector2Int(Mathf.RoundToInt(worldMousePoint.x), Mathf.RoundToInt(worldMousePoint.y));
        cursor.transform.position = new Vector3(cursorPosition.x, cursorPosition.y, cursor.transform.position.z);
    }

    private void OnClickCommand()
    {
        Debug.Log("I received a click!");
        if (DungeonData.PublicData.nodes.ContainsKey(cursorPosition))
        {
            if(DungeonData.PublicData.nodes[cursorPosition].occupyingElement == player.gameObject)
            {
                playerSelected = player.OnSelected();
                Debug.Log("You've clicked on the player!");
            }
            if(playerSelected && DungeonData.PublicData.nodes[cursorPosition].occupyingElement == null)
            {
                if(DungeonData.PublicData.nodes[cursorPosition].occupyingElement == null)
                {
                    player.MoveToTile(cursorPosition);
                }
                playerSelected = false;
                player.OnDeselected();
            }
        }
    }

    private void OnMenuToggle()
    {
        EventSystem.CallEvent(EventType.ON_DUNGEON_MENU_TOGGLED);
    }


    public bool PlayerEndTurn()
    {
        return turnEnded;
    }


}
