using UnityEngine;

public class RetreatState : BaseState<GuardMachine.AIState>
{
    public RetreatState (GuardMachine.AIState key) : base (key)
    {
        StateKey = key;
    }

    public override void EnterState()
    {
        var gMachine = (GuardMachine)MyMachine;


        gMachine.GMat.color = new Color(1,.6f,.6f,1);
        Debug.Log("Recuando, pelos parâmetros do projeto, nunca sai desse estado");
        base.EnterState();
    }

    public override void UpdateState()
    {
        GotoBase();
        base.UpdateState();
    }

    void GotoBase()
    {
        var gMachine = (GuardMachine)MyMachine;

        gMachine.MoveTowards(gMachine.Origin.position);
    }

    public override GuardMachine.AIState GetNextState()
    {
        var gMachine = (GuardMachine)MyMachine;

        if (gMachine.HPCurrent <= 0) return GuardMachine.AIState.Dead;

        //decidir se volta ou fica preso

        return StateKey;
    }
}
