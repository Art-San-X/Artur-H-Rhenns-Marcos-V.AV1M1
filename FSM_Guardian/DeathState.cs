using System;
using UnityEngine;

public class DeathState : BaseState<GuardMachine.AIState>
{
    public DeathState(GuardMachine.AIState key) : base (key)
    {
        StateKey = key;
    }

    public event EventHandler OnGuardianDeath;

    public override void EnterState()
    {
        var gMachine = ((GuardMachine)MyMachine);

        OnGuardianDeath?.Invoke(this, EventArgs.Empty);

        gMachine.GMat.color = Color.black;
        Debug.Log("Guardi�o est� morto");
        base.EnterState();
    }

    public override void UpdateState()
    {
        //l�gica para mudar de sprite / se destruir e instanciar corpo do guardi�o
        base.UpdateState();
    }

    public override GuardMachine.AIState GetNextState()
    {
        //n�o sai do estado morto
        return StateKey;
    }

}
