 using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossStateMachine : StateManager<BossStateMachine.AIState>
{
    public enum AIState
    {
        Idle,
        Chase,
        Attack,
        Rage,
        Recover,
        Dead
    }
    protected override AIState StartingStateKey { get; } = AIState.Idle;

    [Header("Referências")]
    public Transform Origin;
    public Material BossMat;

    public Transform TargetEnemy;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Health Configs")]
    public float HPMax = 20;
    public float HPCurrent = 20, HPRegen = .5f;
    public float HPRageThreshold = .5f, HPFleeThreshold = .2f;
    public int AttacksToRegen = 3, AttacksCounter;
    public bool CanRegen, CanRage;

    [Header("Range Configs")]
    public float RChase = 15, RMelee = 3, RRanged = 10;

    [Header("General Configs")]
    [HideInInspector] public float AttackTimer = 2f;
    public float AttackDelay = 2f, MoveSpeed = 6;
    public float TimeToRage = 2f, RageInterval = 2f;


    protected override Dictionary<AIState, BaseState<AIState>> States { get; set; } =
        new Dictionary<AIState, BaseState<AIState>>()
        {
            {AIState.Idle, new IdleState(AIState.Idle) },
            {AIState.Chase, new BossChaseState(AIState.Chase) },
            {AIState.Attack, new BossAttackState(AIState.Attack) },
            {AIState.Rage, new BossRageState(AIState.Rage) },
            {AIState.Recover, new BossRecoverState(AIState.Recover) },
            { AIState.Dead, new BossDeadState(AIState.Dead) }
        };

    private void Awake()
    {
        var attackState = (BossAttackState)States[AIState.Attack];
        attackState.OnAttack += AttackState_OnAttack;
    }

    private void AttackState_OnAttack(object sender, System.EventArgs e)
    {
        CountAttack();
    }

    protected override void Update()
    {
        TargetEnemy = NearestEnemy();
        base.Update();
    }

    #region Outros Métodos
    //obtém o inimigo mais próximo comparando apenas com objetos na camada
    Transform NearestEnemy()
    {
        var dColliders = Physics.OverlapSphere(transform.position, RChase, _enemyLayer);
        if (dColliders.Count() == 0) return null; //sem inimigos retorno nulo

        var smallestDistance = RChase * 2; // *2 para evitar edge cases
        Collider closestC = null;

        foreach (Collider collider in dColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < smallestDistance)
            {
                //sobrescreve caso ache um mais próximo
                smallestDistance = distance;
                closestC = collider;
            }
        }
        return closestC.transform;
    }
    void CountAttack()
    {
        AttacksCounter++;
    }
    public void TakeDamage()
    {
        HPCurrent--;
    }
    public void MoveTowards(Vector3 targetPos) //movimentação simples, apenas para demo da fsm
    {
        Vector3 dir = ((targetPos  - transform.position) + Vector3.up/2).normalized;
        transform.position += dir * (MoveSpeed * Time.deltaTime);
    }
    public void RegenHP()
    {
        HPCurrent += HPRegen * Time.deltaTime;
        HPCurrent = Mathf.Clamp(HPCurrent, 0, HPMax);
        Debug.Log($"Gaining Health, HP: {HPCurrent} / {HPMax}");
    }
    #endregion
}
