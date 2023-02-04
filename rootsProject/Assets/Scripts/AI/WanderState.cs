using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : AIState
{
    [SerializeField]
    private float wanderRadius;
    [SerializeField]
    private float distanceThreshold;

    private Vector3 destination;
    private Vector3 startPos;

    public override void Enter(NavMeshAgent nav, Animator anim, int id)
    {
        Debug.Log("Enter wander");
        base.Enter(nav, anim,id);
        animator.SetTrigger("Running");
        startPos = agent.gameObject.transform.position;
        SetRandomDestination();
        agent.isStopped = false;
        
    }

    public void SetRandomDestination()
    {
        destination = startPos + new Vector3(Random.Range(-wanderRadius, wanderRadius), 0, Random.Range(-wanderRadius, wanderRadius));
        agent.gameObject.transform.LookAt(destination);
        agent.SetDestination(destination);
    }

    public void Update()
    {
        if(agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            SetRandomDestination();
        }

        if(Vector3.Distance(agent.transform.position, destination) < distanceThreshold)
        {
            SetRandomDestination();
        }

    }
}
