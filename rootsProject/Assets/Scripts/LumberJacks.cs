using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LumberJacks : MonoBehaviour
{
    [SerializeField]
    private AIBase aiBase;
    [SerializeField]
    private Rigidbody mainRigidBody;
    [SerializeField]
    private Rigidbody pelvis;
  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillMe()
    {
        pelvis.AddForce(new Vector3(0,300,0), ForceMode.Impulse);
        mainRigidBody.isKinematic = false;
        aiBase.StopAI();
        Invoke("DisappearMe", 10);

    }

    public void DisappearMe()
    {
        Destroy(gameObject);
    }
}
