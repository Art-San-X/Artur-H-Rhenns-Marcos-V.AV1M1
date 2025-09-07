using System;
using UnityEngine;

public abstract class BaseState<StateE> where StateE : Enum
{
    public StateManager<StateE> MyMachine;

    public StateE StateKey {  get; protected set; }

    public BaseState(StateE key)
    {
        StateKey = key;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public abstract StateE GetNextState();

    public virtual void UpdateState() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }

    public virtual void OnTriggerEnter(Collider collider) { }
    public virtual void OnTriggerStay(Collider collider) { }
    public virtual void OnTriggerExit(Collider collider) { }

    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnCollisionStay(Collision collision) { }
    public virtual void OnCollisionExit(Collision collision) { }

}
