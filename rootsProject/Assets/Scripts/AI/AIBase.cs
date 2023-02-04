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

    protected int audioID;

    private AIState currentState;

    private bool stopped = false;

    private void Awake()
    {
        currentState = startState;
        currentState.Enter(agent, animator, audioID);
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
                    state.Enter(agent, animator, audioID);
                    return; //We found a new state so exit
                }
            }
        }
    }

    public void SetId(int id)
    {
        audioID = id;
    }

    public void StopAI()
    {
        stopped = true;
        animator.enabled = false;
        agent.enabled = false;
        LumberjackAudioMgr.instance.SetLumberjackState(audioID, LumberjackAudioMgr.LumbejackState.dead);
    }

}
