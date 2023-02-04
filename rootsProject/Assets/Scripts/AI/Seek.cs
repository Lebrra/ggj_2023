using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Seek : AIState
{
    [SerializeField]
    private float distance;
    [SerializeField]
    private float lookRate;

    private bool canEnter;
    private bool canExit;

    

    public void Start()
    {
        canEnter = true;
        
    }
    public override void Enter(NavMeshAgent nav, Animator anim, int id)
    {
        base.Enter(nav, anim,id);
        Debug.Log("Enter seek");
        agent.isStopped = true;
        animator.SetTrigger("Idle");
        LumberjackAudioMgr.instance.SetLumberjackState(audioID, LumberjackAudioMgr.LumbejackState.idling);
        canExit = false;
        LookForTarget();
    }

    public void LookForTarget()
    {
        Vector3 fudge = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)) + transform.forward;
        Debug.DrawRay(transform.position, fudge * distance, Color.red);
        Ray ray = new Ray(transform.position, fudge * distance);
        RaycastHit[] rayhits = Physics.RaycastAll(transform.position, fudge * distance, distance);
        foreach (var hit in rayhits)
        {
            Minable root;
            if(hit.collider.TryGetComponent<Minable>(out root))
            {
                LumberjackAudioMgr.instance.SetLumberjackState(audioID, LumberjackAudioMgr.LumbejackState.walking);
                animator.SetTrigger("Running");
                agent.SetDestination(root.transform.position);
                agent.isStopped = false;
                canExit = true;
                Debug.Log("Found target");
                return;
            }

        }

        Invoke("LookForTarget", lookRate);
    }

    public override bool CanExit()
    {
        return canExit;
    }

    public override bool CanEnter()
    {
        return canEnter;
    }
  

}
