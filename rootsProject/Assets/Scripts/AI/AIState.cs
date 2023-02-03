using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    
    protected NavMeshAgent agent;
   
    protected Animator animator;
    [field: SerializeField]
    public List<AIState> nextStates { get; set; }

    public void Awake()
    {
        if (nextStates.Count == 0)
        {
            Debug.LogWarning("AI State has no next state!");
        }
    }

    public virtual bool CanEnter()
    {
        return true;
    }

    public virtual bool CanExit()
    {
        return true;
    }

    public virtual void Enter(NavMeshAgent nav, Animator anim)
    {
        animator = anim;
        agent = nav;
        
    }

    public virtual void Exit()
    {
   
    }
}
