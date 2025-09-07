using System;
using UnityEngine;

public class BossAttackState : BaseState<BossStateMachine.AIState>
{
    public BossAttackState(BossStateMachine.AIState key) : base(key)
    {
        StateKey = key;
    }

    public event EventHandler OnAttack;
    bool _attackEnded = false, _isAttacking, _isAttackingm;
    float _attackTime = 1.5f;

    public override void EnterState()
    {
        var bMachine = (BossStateMachine)MyMachine;
        OnAttack?.Invoke(this, EventArgs.Empty);
        _attackEnded = false;
        _isAttacking = false;
        bMachine.BossMat.color = Color.red;
        _attackTime = 1.5f;
        Debug.Log("começando decisão de ataque");
        base.EnterState();
    }

    public override void UpdateState()
    {
        var bMachine = (BossStateMachine)MyMachine;

        var enemyDist = Vector3.Distance(bMachine.transform.position, bMachine.TargetEnemy.position);

        if (bMachine.RMelee < enemyDist && enemyDist < bMachine.RRanged || _isAttacking)
        {
            RangedAttack();
        }
        else if (enemyDist <= bMachine.RMelee || _isAttackingm)
        {
            MeleeAttack();
        }
        else _attackEnded = true; //se inimigo estiver à distância > 10 ataque apenas encerra
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
            _attackEnded = true;
            _isAttackingm = false;
        }
    
    }
    void RangedAttack()
    {
        if (_attackTime > 0)
        {
            _isAttacking = true;
            _attackTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Ataque à distância");
            _attackEnded = true;
            _isAttacking = false;
        }
    }

    public override BossStateMachine.AIState GetNextState()
    {
        var bMachine = (BossStateMachine)MyMachine;
        if (_attackEnded)
        {
            if (bMachine.HPCurrent <= bMachine.HPMax * bMachine.HPRageThreshold) return BossStateMachine.AIState.Rage;
            if (bMachine.HPCurrent <= bMachine.HPMax * bMachine.HPFleeThreshold && bMachine.CanRegen) return BossStateMachine.AIState.Recover;
            if (bMachine.HPCurrent <= 0) return BossStateMachine.AIState.Dead;

            return BossStateMachine.AIState.Chase;
        }

        return StateKey;
    }
}
