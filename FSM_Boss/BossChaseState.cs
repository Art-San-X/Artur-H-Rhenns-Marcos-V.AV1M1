using UnityEngine;

public class BossChaseState : BaseState<BossStateMachine.AIState>
{
    public BossChaseState(BossStateMachine.AIState key) : base(key)
    {
        StateKey = key;
    }
    public override void EnterState()
    {
        var bMachine = (BossStateMachine)MyMachine;
        Debug.Log("Começou Perseguição Chase");
        bMachine.AttackTimer = bMachine.AttackDelay;
        if(bMachine.TimeToRage <= 0) bMachine.TimeToRage = bMachine.RageInterval;

        bMachine.BossMat.color = new Color(.8f,.2f,.8f,1);
        base.EnterState();
    }
    public override void UpdateState()
    {
        var bMachine = (BossStateMachine)MyMachine;

        TimerToAttack();
        TargetChase();
        if (bMachine.AttacksCounter > bMachine.AttacksToRegen)
        {
            bMachine.CanRegen = true;
            bMachine.AttacksCounter = 0;
        }
        base.UpdateState();
    }
    private void TimerToAttack()
    {
        var bMachine = (BossStateMachine)MyMachine;
        if (bMachine.AttackTimer > 0) bMachine.AttackTimer -= Time.deltaTime;
        if (bMachine.TimeToRage > 0) bMachine.TimeToRage -= Time.deltaTime;
        else bMachine.CanRage = true;
    }

    private void TargetChase()
    {
        var bMachine = (BossStateMachine)MyMachine;

        //se o Boss chegar muito perto ataca insta
        if (Vector3.Distance(bMachine.transform.position, bMachine.TargetEnemy.position + Vector3.up/2) > 1f) bMachine.MoveTowards(bMachine.TargetEnemy.position);
        else bMachine.AttackTimer = 0;
    }
    public override BossStateMachine.AIState GetNextState()
    {
        var bMachine = (BossStateMachine)MyMachine;

        if (bMachine.TargetEnemy != null)
        if (Vector3.Distance(bMachine.transform.position, bMachine.TargetEnemy.position) <= bMachine.RRanged && bMachine.AttackTimer <= 0) return BossStateMachine.AIState.Attack;
        if (bMachine.HPCurrent <= bMachine.HPMax * bMachine.HPRageThreshold && bMachine.TimeToRage <= 0) return BossStateMachine.AIState.Rage;
        if (bMachine.CanRegen && bMachine.HPCurrent <= bMachine.HPMax * bMachine.HPFleeThreshold && bMachine.CanRegen) return BossStateMachine.AIState.Recover;
        if (bMachine.HPCurrent <= 0) return BossStateMachine.AIState.Dead;

        return StateKey;
    }
}
