using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<StateE> : MonoBehaviour where StateE : Enum
{
    //associa as classes dos estados ao seu nome no Enum
    protected abstract Dictionary<StateE, BaseState<StateE>> States { get; set; }
    protected BaseState<StateE> CurrentState; //mantém o controle de qual classe está atuando
    protected abstract StateE StartingStateKey { get; } //define uma classe como base inicial

    private void TransitionToState(StateE nextStateKey)
    {
        CurrentState.ExitState();
        CurrentState = States[nextStateKey];
        CurrentState.EnterState();
    }

    private void Start()
    {
        CurrentState = States[StartingStateKey];
        foreach (var state in States)
        {
            state.Value.MyMachine = this;
        }
        CurrentState.EnterState();
    }

    protected virtual void Update()
    {
        var nextStateKey = CurrentState.GetNextState();
        if(nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else
        {
            TransitionToState(nextStateKey);
        }
    }

    private void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }

    private void LateUpdate()
    {
        CurrentState.LateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CurrentState.OnCollisionEnter(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        CurrentState.OnCollisionStay(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        CurrentState.OnCollisionExit(collision);
    }
}

