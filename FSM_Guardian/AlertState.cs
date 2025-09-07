using System;
using UnityEngine;

public class AlertState : BaseState<GuardMachine.AIState>
{
    public AlertState(GuardMachine.AIState key) : base(key)
    {
        StateKey = key;
    }

    public event EventHandler OnGuardianAlertEnter;

    public override void EnterState()
    {
        var gMachine = ((GuardMachine)MyMachine);


        gMachine.GMat.color = new Color(1,.5f,0,1);
        OnGuardianAlertEnter?.Invoke(this, EventArgs.Empty); //evento para usos externos
        Debug.Log("Is Alert");
        base.EnterState();
    }

    public override void UpdateState()
    {
        var gMachine = (GuardMachine)MyMachine;

        if (gMachine.TargetEnemy != null) 
        gMachine.transform.LookAt(gMachine.TargetEnemy);
        
        base.UpdateState();
    }

    public override GuardMachine.AIState GetNextState()
    {
        var gMachine = (GuardMachine)MyMachine;
        
        if(gMachine.TargetEnemy == null) return GuardMachine.AIState.Patrol;

        float distToEnemy = Vector3.Distance(gMachine.transform.position, gMachine.TargetEnemy.position);

        //se sair do alcance volta para patrulha
        if (distToEnemy > gMachine.RAlert)  return GuardMachine.AIState.Patrol;

        //se entrar no alcance de perseguição, começar
        if (distToEnemy < gMachine.RChase)  return GuardMachine.AIState.Chase;  

        if (gMachine.HPCurrent <= 0) return GuardMachine.AIState.Dead;
        
        return StateKey;
    }
}
