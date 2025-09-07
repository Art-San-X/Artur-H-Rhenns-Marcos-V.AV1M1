using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReinforcementsState : BaseState<GuardMachine.AIState>
{
    public ReinforcementsState (GuardMachine.AIState key) : base (key)
    {
        StateKey = key;
    }

    public event EventHandler OnCallRef;

    float _stateDurationTimer = .5f;

    public override void EnterState()
    {
        var gMachine = ((GuardMachine)MyMachine);


        gMachine.GMat.color = new Color(.2f,.5f,.7f,1);
        OnCallRef?.Invoke(this, EventArgs.Empty);
        base.EnterState();
    }

    public override void UpdateState()
    {
        Debug.Log("Chamando Reforços (Que nunca chegam)");

        if (_stateDurationTimer > 0) _stateDurationTimer -= Time.deltaTime;
        else CallRef();
        base.UpdateState();
    }

    private void CallRef()
    {
        ((GuardMachine)MyMachine).CalledReinforce = true;
        //lógica para instanciar outro guardião
    }


    public override GuardMachine.AIState GetNextState()
    {
        var gMachine = (GuardMachine)MyMachine;

        if (gMachine.CalledReinforce) return GuardMachine.AIState.Chase;

        if (gMachine.HPCurrent <= 0) return GuardMachine.AIState.Dead;

        return StateKey;
    }
}
