using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MineState : AIState
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float rate;
    [SerializeField]
    private GameObject axe;

    private Minable current;
    private bool done;

    public override void Enter(NavMeshAgent nav, Animator anim)
    {
       
        Debug.Log("Enter Mine");
        base.Enter(nav, anim);
        if (current != null)
        {

            agent.isStopped = true;
            agent.transform.LookAt(current.transform.position);
            animator.SetTrigger("Mining");
            axe.SetActive(true);
            done = false;
            Damage();
            Invoke("AllowExit", rate*3);
        }
    }


    public void Damage()
    {
        if(current != null)
        {
            current.ApplyDamage(damage);
            Invoke("Damage", rate);
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
        axe.SetActive(false);
       
        current = null;
        done = true;
    }


    public void OnCollisionEnter(Collision collision)
    {
        Minable collectable = collision.gameObject.GetComponent<Minable>();
        if (collectable != null && current == null)
        {
            current = collectable;
            
            done = false;
        }
    }

}
