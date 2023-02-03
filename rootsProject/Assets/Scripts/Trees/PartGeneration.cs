using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;

public class PartGeneration : MonoBehaviour
{
    [SerializeField]
    float growTime = 0.1F;

    [SerializeField]
    Transform spawnpoint;
    [SerializeField]
    TreeChunkLogic partPrefab;

    Routine growRoot;

    private void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!growRoot.Exists())
            {
                growRoot.Replace(SpawnRootPart(new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))));
            }
        }
    }

    IEnumerator SpawnRootPart(Vector2 playerInput)
    {
        // validation for 0 / -1
        var degree = Mathf.Atan(playerInput.y / playerInput.x) * Mathf.Rad2Deg;
        if (playerInput.x > 0) degree -= 180F;
        Debug.Log(degree);

        var newPart = Instantiate(partPrefab, spawnpoint.position, Quaternion.Euler(0, degree, 0));
        spawnpoint = newPart.EndtPoint;
        yield return growTime;
    }
}
