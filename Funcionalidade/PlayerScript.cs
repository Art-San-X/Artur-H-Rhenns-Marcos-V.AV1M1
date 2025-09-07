using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("Vair�veis")]
    [SerializeField] InputAction _attackAction;
    [SerializeField] Transform _target;
    [SerializeField] BossStateMachine _bossBrain;
    [SerializeField] GuardMachine _gBrain;


    private void Start()
    {
        
        _attackAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (_attackAction.WasPressedThisFrame())
        {
            if (Vector3.Distance(transform.position, _target.position) < 10)
            {
                if (_bossBrain != null) _bossBrain.TakeDamage();
                if (_gBrain != null) _gBrain.TakeDamage();
                Debug.Log("Causou Dano");
            }
            else Debug.Log("T� muito longe");
        }
    }
}
