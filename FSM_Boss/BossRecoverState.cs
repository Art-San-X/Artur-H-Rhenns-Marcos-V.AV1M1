using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BossRecoverState : BaseState<BossStateMachine.AIState>
{
    public BossRecoverState(BossStateMachine.AIState key) : base(key)
    {
        StateKey = key;
    }

    bool _gotToCentre;

    public override void EnterState()
    {
        _gotToCentre = false;
        base.EnterState();
    }

    public override void UpdateState()
    {
        Debug.Log("Going to center");
        if (GotoOrigin())
        {
            Debug.Log("Regenerating");
            Regen();
        }
        base.UpdateState();
    }

    private bool GotoOrigin()
    {
        var bMachine = (BossStateMachine)MyMachine;

        if (Vector3.Distance(bMachine.transform.position, bMachine.Origin.position + Vector3.up/2) > .5f)
        {
            bMachine.MoveTowards(bMachine.Origin.position);
            bMachine.BossMat.color = Color.yellow;
            return false;
        }
        else
        {
            bMachine.BossMat.color = Color.green;
            _gotToCentre = true;
            bMachine.CanRegen = false;
            return true;
        }
    }

    private void Regen()
    {
        var bMachine = (BossStateMachine)MyMachine;

        bMachine.HPCurrent += bMachine.HPRegen * Time.deltaTime;
        bMachine.HPCurrent = Mathf.Clamp(bMachine.HPCurrent, 0f, bMachine.HPMax);
    }

    public override BossStateMachine.AIState GetNextState()
    {
        var bMachine = (BossStateMachine)MyMachine;

        if (bMachine.HPCurrent >= bMachine.HPMax) return BossStateMachine.AIState.Chase;
        if (bMachine.TargetEnemy != null && _gotToCentre) 
            if(Vector3.Distance(bMachine.transform.position, bMachine.TargetEnemy.position) <= bMachine.RMelee) return BossStateMachine.AIState.Chase;
        if (bMachine.HPCurrent <= 0) return BossStateMachine.AIState.Dead;


        return StateKey;
    }
}
