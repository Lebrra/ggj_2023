using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    
    protected NavMeshAgent agent;
   
    protected Animator animator;
    protected int audioID;
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

    public virtual void Enter(NavMeshAgent nav, Animator anim, int id)
    {
        animator = anim;
        agent = nav;
        audioID = id;
        
    }

    public virtual void Exit()
    {
   
    }
}
