using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    private AIState startState;

    private AIState currentState;

    public void InitalizeStateMachine(NavMeshAgent nav, Animator anim)
    {

    }
}
