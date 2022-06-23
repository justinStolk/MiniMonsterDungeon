using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool Enter { get; set; }

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
        PlayerTurnState playTurn = new PlayerTurnState(this);
        EnemyTurnState enemTurn = new EnemyTurnState(this);

        stateMachine = new StateMachine(prep, playTurn, enemTurn);
        stateMachine.AddTransition(new StateTransition(prep, playTurn, GoToDungeon));
        //stateMachine.AddTransition(null, prep, PLAYERISDEAD);

        stateMachine.SwitchState(prep);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
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
