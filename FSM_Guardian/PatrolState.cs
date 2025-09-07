using UnityEngine;

public class PatrolState : BaseState<GuardMachine.AIState>
{ 
    public PatrolState(GuardMachine.AIState key) : base(key)
    {
        StateKey = key;
    }

    public override void EnterState()
    {
        var gMachine = ((GuardMachine)MyMachine);
        gMachine.GMat.color = Color.cyan;

        base.EnterState();
    }

    public override void UpdateState()
    {
        var gMachine = ((GuardMachine)MyMachine);

        if (gMachine.PatrolPoints.Count > 0) //se tiver pontos de patrulha
        {
            Transform patrolTarget = gMachine.PatrolPoints[gMachine.CurrentPatrolIndex]; //obtém alvo atual
            gMachine.MoveTowards(patrolTarget.position); //vai à ele

            if(Vector3.Distance(gMachine.transform.position, patrolTarget.position + Vector3.up/2) < .5f)
            {
                gMachine.CurrentPatrolIndex = (gMachine.CurrentPatrolIndex + 1) % gMachine.PatrolPoints.Count; //operador modular % (sobra da divisão) garante ciclicidade
                Debug.Log("Indo para ponto de patrulha " + gMachine.CurrentPatrolIndex );
            }
        }
        base.UpdateState();
    }

    public override GuardMachine.AIState GetNextState()
    {
        var gMachine = ((GuardMachine)MyMachine);

        if(gMachine.TargetEnemy != null)
        if (Vector3.Distance(gMachine.transform.position, gMachine.TargetEnemy.position) < gMachine.RAlert)
        return GuardMachine.AIState.Alert;
        

        return StateKey;
    }
}
