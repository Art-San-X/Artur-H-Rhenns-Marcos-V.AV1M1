using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BaseState<BossStateMachine.AIState>
{
    public IdleState (BossStateMachine.AIState key) : base (key)
    {
        StateKey = key;
    }

    public override void UpdateState()
    {
        var bMachine = (BossStateMachine)MyMachine;
        bMachine.BossMat.color = Color.green;
        base.UpdateState();
    }

    public override BossStateMachine.AIState GetNextState()
    {
        var bMachine = (BossStateMachine)MyMachine;

        if (bMachine.TargetEnemy != null)
        if (Vector3.Distance(bMachine.transform.position, bMachine.TargetEnemy.position) < bMachine.RChase) return BossStateMachine.AIState.Chase;
        if (bMachine.HPCurrent <= 0) return BossStateMachine.AIState.Dead;

        return StateKey;
    }
}
