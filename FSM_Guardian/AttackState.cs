using System;
using UnityEngine;

public class AttackState :BaseState<GuardMachine.AIState>
{
    public AttackState (GuardMachine.AIState key) : base (key)
    {
        StateKey = key;
    }

    public event EventHandler OnGuardianMeleeAttack;
    public event EventHandler OnGuardianRangedAttack;

    bool _attackEnded = false, _isAttackingR, _isAttackingm;
    float _attackTime = 1.5f, _attackDuration = 1.5f;

    public override void EnterState()
    {
        var gMachine = (GuardMachine)MyMachine;

        _attackEnded = false;
        _isAttackingR = false;
        _isAttackingm = false;
        _attackTime = _attackDuration;

        gMachine.GMat.color = Color.red;
        Debug.Log("Started Attacking");
        _attackEnded = false;
        base.EnterState();
    }

    public override void UpdateState()
    {
        var gMachine = (GuardMachine)MyMachine;

        var enemyDist = Vector3.Distance(gMachine.transform.position, gMachine.TargetEnemy.position);

        if (gMachine.RMelee < enemyDist && enemyDist < gMachine.RRanged || _isAttackingR)
        {
            RangedAttack();
        }
        else if (enemyDist < gMachine.RMelee || _isAttackingm)
        {
            MeleeAttack();
        }


        base.UpdateState();
    }

    void MeleeAttack()
    {
        if (_attackTime > 0)
        {
            _isAttackingm = true;
            _attackTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Ataque Melee");
            OnGuardianMeleeAttack?.Invoke(this, EventArgs.Empty);
            _attackEnded = true;
            _isAttackingm = false;
        }

    }
    void RangedAttack()
    {
        if (_attackTime > 0)
        {
            _isAttackingR = true;
            _attackTime -= Time.deltaTime;
        }
        else
        {
            OnGuardianRangedAttack?.Invoke(this, EventArgs.Empty);
            Debug.Log("Ataque à distância");
            _attackEnded = true;
            _isAttackingR = false;
        }
    }

    public override void ExitState()
    {
        var gMachine = (GuardMachine)MyMachine;
        gMachine.AttackTimer = gMachine.AttackDelay; //reinicia o timer para atacar

        base.ExitState();
    }

    public override GuardMachine.AIState GetNextState()
    {
        var gMachine = (GuardMachine)MyMachine;

        if (_attackEnded)
        {

            if(gMachine.HPCurrent <= 0 ) return GuardMachine.AIState.Dead;

            if (gMachine.HPCurrent <= gMachine.HPMax * gMachine.HPReinforceThreshold && !gMachine.CalledReinforce)
                return GuardMachine.AIState.Reinforce;

            if (gMachine.HPCurrent <= gMachine.HPMax * gMachine.HPFleeThreshold)
                return GuardMachine.AIState.Flee;

            return GuardMachine.AIState.Chase;
        }

        return StateKey;
    }
}
