using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Cursor { get; set; }
    public Player Player { get; set; }
    public bool Enter { get; set; }
    public bool PlayerDied = false;

    public int SkillPoints { get; private set; }

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
        DungeonExitState exit = new DungeonExitState(this);

        stateMachine = new StateMachine(prep, playTurn, enemTurn);
        stateMachine.AddTransition(new StateTransition(prep, entry, GoToDungeon));
        stateMachine.AddTransition(new StateTransition(entry, playTurn, entry.DungeonInitialized));
        stateMachine.AddTransition(new StateTransition(playTurn, enemTurn, playTurn.PlayerEndTurn));
        stateMachine.AddTransition(new StateTransition(enemTurn, playTurn, enemTurn.AllEnemiesFinishedTurn));
        stateMachine.AddTransition(new StateTransition(null, exit, PlayerIsDead));
        stateMachine.AddTransition(new StateTransition(exit, prep, exit.Exited));
        //stateMachine.AddTransition(null, prep, PLAYERISDEAD);

        EventSystem.Subscribe(EventType.ON_PLAYER_DIE, () => PlayerDied = true);

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

    public void GetSkillPoints(int amount)
    {
        SkillPoints += amount;
    }

    public bool SpendSkillPoints(int amount)
    {
        if(SkillPoints >= amount)
        {
            SkillPoints -= amount;
            return true;
        }
        return false;
    }
    private bool PlayerIsDead()
    {
        return PlayerDied;
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
