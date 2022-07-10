using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Cursor { get; set; }
    public Player Player { get; set; }
    public bool Enter { get; set; }

    public delegate void OnCommandPressed();
    public OnCommandPressed OnConfirmed;
    public OnCommandPressed OnMenuToggle;


    private StateMachine stateMachine;

    private void Awake()
    {

        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {

        PreparationState prep = new PreparationState(this);
        DungeonEnterState entry = new DungeonEnterState(this);
        PlayerTurnState playTurn = new PlayerTurnState(this);
        EnemyTurnState enemTurn = new EnemyTurnState(this);

        stateMachine = new StateMachine(prep, playTurn, enemTurn);
        stateMachine.AddTransition(new StateTransition(prep, entry, GoToDungeon));
        stateMachine.AddTransition(new StateTransition(entry, playTurn, entry.DungeonInitialized));
        stateMachine.AddTransition(new StateTransition(playTurn, enemTurn, playTurn.PlayerEndTurn));
        //stateMachine.AddTransition(null, prep, PLAYERISDEAD);

        stateMachine.SwitchState(prep);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnConfirmed?.Invoke();
        }
        if (Input.GetButtonDown("Submit"))
        {
            OnMenuToggle?.Invoke();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }

    public void EndPlayerTurn()
    {
        EventSystem.CallEvent(EventType.ON_PLAYER_TURN_ENDED);
    }

    private bool GoToDungeon()
    {
        if (Enter)
        {
            Enter = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntoTheDungeon");
            return true;
        }
        return false;
    }
}
