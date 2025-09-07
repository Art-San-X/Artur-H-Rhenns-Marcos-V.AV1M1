using UnityEngine;

public class BossDeadState : BaseState<BossStateMachine.AIState>
{
    public BossDeadState(BossStateMachine.AIState key) : base(key)
    {
        StateKey = key;
    }

    public override void EnterState()
    {
        Debug.Log("boss est� morto, nunca sai do estado");
        base.EnterState();
    }

    public override void UpdateState()
    {
        var bMachine = (BossStateMachine)MyMachine;

        bMachine.BossMat.color = Color.black;
    }

    public override BossStateMachine.AIState GetNextState()
    {
        //n�o sai do estado morto
        
        return StateKey;
    }
}
