using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : AIState
{
    [SerializeField]
    private float minTime;
    [SerializeField]
    private float maxTime;
    [SerializeField]
    private float minPauseTime;
    [SerializeField]
    private float maxPauseTime;

    private bool canEnter;
    private bool canExit;

    public void Start()
    {
        Invoke("AllowEnter", Random.Range(minTime, maxTime));
    }
    public override void Enter(NavMeshAgent nav, Animator anim, int id)
    {
        base.Enter(nav, anim,id);
        agent.isStopped = true;
        animator.SetTrigger("Idle");
        canExit = false;
        canEnter = false;
        Invoke("AllowExit", Random.Range(minPauseTime, maxPauseTime));
    }

    public override bool CanExit()
    {
        return canExit;
    }

    public override bool CanEnter()
    {
        return canEnter;
    }
    public void AllowExit()
    {
        canExit = true;
        Invoke("AllowEnter", Random.Range(minTime, maxTime));
    }

    public void AllowEnter()
    {
        canEnter = true;
    }

}
