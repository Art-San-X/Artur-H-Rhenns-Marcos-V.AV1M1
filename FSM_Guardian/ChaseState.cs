using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ChaseState : BaseState<GuardMachine.AIState>
{
    public ChaseState(GuardMachine.AIState key) : base(key)
    {
        StateKey = key;
    }

    public event EventHandler OnGuardianEnterChase;

    public override void EnterState()
    {
        var gMachine = (GuardMachine)MyMachine;

        gMachine.GMat.color = Color.magenta;

        gMachine.AttackTimer = gMachine.AttackDelay;

        OnGuardianEnterChase?.Invoke(this, EventArgs.Empty);
        Debug.Log("Começou Chase");
        base.EnterState();
    }

    public override void UpdateState()
    {
        var gMachine = (GuardMachine)MyMachine;
        if (gMachine.TargetEnemy != null)
        {
            //perseguir o jogador e operar timer para atacar
            TimerToAttack();
            TargetChase();
        }
        base.UpdateState();
    }
    private void TimerToAttack()
    {
        var gMachine = (GuardMachine)MyMachine;

        if (gMachine.AttackTimer > 0) gMachine.AttackTimer -= Time.deltaTime;
    }
    private void TargetChase()
    {
        var gMachine = (GuardMachine)MyMachine;

        //se o guardião chegar muito perto ataca insta
        if (Vector3.Distance(gMachine.transform.position, gMachine.TargetEnemy.position) > 1f)gMachine.MoveTowards(gMachine.TargetEnemy.position);
        else gMachine.AttackTimer = 0;
    }


    public override GuardMachine.AIState GetNextState()
    {
        var gMachine = (GuardMachine)MyMachine;
        if (gMachine.HPCurrent <= 0) return GuardMachine.AIState.Dead;

        if(gMachine.TargetEnemy == null) return GuardMachine.AIState.Patrol;

        if (gMachine.AttackTimer <= 0) return GuardMachine.AIState.Attack;

        if (gMachine.HPCurrent <= gMachine.HPMax * gMachine.HPReinforceThreshold && !gMachine.CalledReinforce)
            return GuardMachine.AIState.Reinforce;

        if (gMachine.HPCurrent <= gMachine.HPMax * gMachine.HPFleeThreshold)
            return GuardMachine.AIState.Flee;

        if (Vector3.Distance(gMachine.transform.position, gMachine.TargetEnemy.position) > gMachine.RRanged)
            return GuardMachine.AIState.Alert;

        return StateKey;
    }
}
