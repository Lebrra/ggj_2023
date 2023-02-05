using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberJackSpawning : MonoBehaviour
{
    [SerializeField]
    private AIBase lumberJackPrefab;
    [SerializeField]
    private float baseSpawnRate;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", baseSpawnRate * Mathf.Cos(GameStateManager.CurrentNormalizedGameTime));
    }


    public void Spawn()
    {
        AIBase lumberJack = Instantiate<AIBase>(lumberJackPrefab, transform.position, transform.rotation);
        // They need to register for sounds
        
        int myId = LumberjackAudioMgr.instance.RegisterLumberjack(LumberjackAudioMgr.LumbejackState.idling);
        lumberJack.SetId(myId);
        Invoke("Spawn", baseSpawnRate * Mathf.Sin(GameStateManager.CurrentNormalizedGameTime));
    }
}
