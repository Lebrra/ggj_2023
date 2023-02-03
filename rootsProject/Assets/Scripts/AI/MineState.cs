using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MineState : AIState
{
    private Minable current;
   
    private bool done;

    public override void Enter(NavMeshAgent nav, Animator anim)
    {
        Debug.Log("Enter Mine");
        base.Enter(nav, anim);
        if (current != null)
        {

            agent.isStopped = true;
            animator.SetTrigger("Mining");
            done = false;
            Invoke("AllowExit", current.timeToMine);
        }
    }

    public override bool CanEnter()
    {
        return (current != null);
    }

    public override bool CanExit()
    {
        return done;
    }

    private void AllowExit()
    {
        if (current != null)
        {
            Destroy(current.gameObject);
        }
        current = null;
        done = true;
    }


    public void OnCollisionEnter(Collision collision)
    {
        Minable collectable = collision.gameObject.GetComponent<Minable>();
        if (collectable != null && current == null)
        {
            current = collectable;
            agent.transform.LookAt(current.transform.position);
        }
    }

}
