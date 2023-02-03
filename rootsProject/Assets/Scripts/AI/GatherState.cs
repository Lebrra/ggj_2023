using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GatherState : AIState
{
    private Collectable currentCollectable;
    private bool done;

    public override void Enter(NavMeshAgent nav, Animator anim)
    {
        Debug.Log("Enter gather");
        base.Enter(nav, anim);
        if (currentCollectable != null)
        {
          
            agent.isStopped = true;
            animator.SetTrigger("Gathering");
            float animLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            done = false;
            Invoke("AllowExit", animLength);
        }
    }

    public override bool CanEnter()
    {
        return (currentCollectable != null);
    }

    public override bool CanExit()
    {
        return done;
    }

    private void AllowExit()
    {
        if (currentCollectable != null)
        {
            Destroy(currentCollectable.gameObject);
        }
        currentCollectable = null;
        done = true;
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        Collectable collectable = collision.gameObject.GetComponent<Collectable>();
        if(collectable != null && currentCollectable == null)
        {
            currentCollectable = collectable;
            agent.transform.LookAt(currentCollectable.transform.position);
           
        }
        
    }



  
}
