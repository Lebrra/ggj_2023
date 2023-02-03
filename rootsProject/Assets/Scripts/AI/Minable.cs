using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minable : MonoBehaviour
{

    [SerializeField]
    public float health;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
