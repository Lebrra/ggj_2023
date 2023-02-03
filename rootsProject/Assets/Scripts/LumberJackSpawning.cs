using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberJackSpawning : MonoBehaviour
{
    [SerializeField]
    private GameObject lumberJackPrefab;
    [SerializeField]
    private float baseSpawnRate;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", baseSpawnRate * Mathf.Cos(GameStateManager.CurrentNormalizedGameTime));
    }


    public void Spawn()
    {
        Instantiate(lumberJackPrefab, transform.position, transform.rotation);
        Invoke("Spawn", baseSpawnRate * Mathf.Sin(GameStateManager.CurrentNormalizedGameTime));
    }
}
