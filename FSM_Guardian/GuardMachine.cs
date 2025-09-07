using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GuardMachine : StateManager<GuardMachine.AIState>
{
    public enum AIState
    {
        Patrol,
        Alert,
        Chase,
        Attack,
        Reinforce,
        Flee,
        Dead
    }

    protected override AIState StartingStateKey { get; } = AIState.Patrol;

    [Header("Referências")]
    public Transform Origin;
    public List<Transform> PatrolPoints;
    public Material GMat;

    public int CurrentPatrolIndex;
    public Transform TargetEnemy;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Health Configs")]
    public float HPMax = 10;
    public float HPCurrent = 10, HPRegen = .5f;
    public float HPReinforceThreshold = .4f, HPFleeThreshold = .2f;

    [Header("Range Configs")]
    public float RAlert = 20;
    public float RChase = 10, RMelee = 3, RRanged = 10;

    [Header("General Configs")]
    [HideInInspector] public float AttackTimer = 3f; 
    public float AttackDelay = 3f, MoveSpeed = 5;
    public bool CalledReinforce = false;

    protected override Dictionary<AIState, BaseState<AIState>> States { get; set; } =
    new Dictionary<AIState, BaseState<AIState>>()
    {
        {AIState.Patrol, new PatrolState(AIState.Patrol)},
        {AIState.Alert, new AlertState(AIState.Alert)},
        {AIState.Chase, new ChaseState(AIState.Chase)},
        {AIState.Attack, new AttackState(AIState.Attack)},
        {AIState.Reinforce, new ReinforcementsState(AIState.Reinforce)},
        {AIState.Flee, new RetreatState(AIState.Flee)},
        {AIState.Dead, new DeathState(AIState.Dead) }
    };

    protected override void Update()
    {
        TargetEnemy = NearestEnemy();
        base.Update();
    }

    #region Outros Métodos
    //obtém o inimigo mais próximo comparando apenas com objetos na camada
    Transform NearestEnemy()
    {   
        var dColliders = Physics.OverlapSphere(transform.position,RAlert, _enemyLayer);
        if (dColliders.Count() == 0) return null; //sem inimigos retorno nulo

        var smallestDistance = RAlert * 2; // *2 para evitar edge cases
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

    public void TakeDamage()
    {
        HPCurrent--;
    }

    public void MoveTowards(Vector3 targetPos) //movimentação simples, apenas para demo da fsm
    {
        Vector3 dir = (targetPos - transform.position + Vector3.up/2).normalized;
        transform.position += dir * (MoveSpeed * Time.deltaTime);
        transform.LookAt(transform.position + dir);
    }

    //seria implementado, mas não há nescessidade e não houve tempo
    //public void RegenHP()
    //{
    //    HPCurrent += HPRegen * Time.deltaTime;
    //    HPCurrent = Mathf.Clamp(HPCurrent, 0, HPMax);
    //    Debug.Log($"Gaining Health, HP: {HPCurrent} / {HPMax}");
    //}
    #endregion
}
