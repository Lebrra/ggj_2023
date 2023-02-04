using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AIState startState;

    private AIState currentState;

    private bool stopped = false;

    private void Awake()
    {
        currentState = startState;
        currentState.Enter(agent, animator);
        stopped = false;
    }

    private void Update()
    {
        if(stopped)
        {
            return;
        }

        if(currentState.CanExit())
        {
            foreach(AIState state in currentState.nextStates)
            {
                if(state.CanEnter())
                {
                    currentState = state;
                    state.Enter(agent, animator);
                    return; //We found a new state so exit
                }
            }
        }
    }

    public void StopAI()
    {
        stopped = true;
        animator.enabled = false;
        agent.enabled = false;
    }

}
