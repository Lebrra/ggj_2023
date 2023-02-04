using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMinable : Minable
{
    public override void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("THE TREE HAS DIED!");
            GameStateManager.GameOver(false);
        }
    }
}
