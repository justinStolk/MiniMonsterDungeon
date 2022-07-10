using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private List<StateTransition> allTransitions = new ();
    private List<StateTransition> activeTransitions = new ();
    private IState currentState;

    private Dictionary<Type, IState> allStates = new();

    public StateMachine(params IState[] states)
    {
        foreach(IState state in states)
        {
            allStates.Add(state.GetType(), state);
        }
    }

    public void OnFixedUpdate()
    {
        foreach (StateTransition transition in activeTransitions)
        {
            if (transition.Evaluate())
            {
                SwitchState(transition.ToState);
                return;
            }
        }
        currentState?.OnUpdate();
    }
    public void SwitchState(IState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        Debug.Log(newState.GetType().ToString());
        activeTransitions = allTransitions.FindAll(trn => trn.FromState == currentState || trn.FromState == null);
        currentState.OnEnter();
    }
    public void AddState(IState state)
    {
        allStates.Add(state.GetType(), state);
    }

    public void AddTransition(StateTransition transition)
    {
        allTransitions.Add(transition);
    }
}

public class StateTransition
{
    public IState FromState;
    public IState ToState;
    public Func<bool> Condition;

    public StateTransition(IState fromState, IState toState, Func<bool> condition)
    {
        FromState = fromState;
        ToState = toState;
        Condition = condition;
    }

    public bool Evaluate()
    {
        return Condition();
    }

}


public abstract class State<T> : IState where T : MonoBehaviour
{
    public T Owner { get; protected set; }

    public State(T owner)
    {
        Owner = owner;
    }
    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void OnUpdate() { }
}