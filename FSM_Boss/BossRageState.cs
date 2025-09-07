using UnityEngine;

public class BossRageState : BaseState<BossStateMachine.AIState>
{
    public BossRageState(BossStateMachine.AIState key) : base(key)
    {
        StateKey = key;
    }

    float attackDuration = 1f;
    bool attacked;

    public override void EnterState()
    {
        attackDuration = 1f;
        attacked = false;
        base.EnterState();
    }

    public override void UpdateState()
    {
       
        AreaAttack();
        
        base.UpdateState();
    }

    void AreaAttack()
    {
        var bMachine = (BossStateMachine)MyMachine;

        if(attackDuration > 0)
        {
            bMachine.BossMat.color = Color.yellow;
            Debug.Log("Atacando em área");
            attackDuration -= Time.deltaTime;
        }
        else 
        { 
            attacked = true;
            bMachine.CanRage = false;
        }
    }

    public override BossStateMachine.AIState GetNextState()
    {
        var bMachine = (BossStateMachine)MyMachine;

        if (bMachine.CanRegen && bMachine.HPCurrent <= bMachine.HPMax * bMachine.HPFleeThreshold) return BossStateMachine.AIState.Recover;
        if (bMachine.HPCurrent <= 0) return BossStateMachine.AIState.Dead;
        if (attacked) return BossStateMachine.AIState.Chase;


        return StateKey;
    }
}
 