using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> rockVariants;
    // size 100 - 300 looks good
    // radius range: 10 - 40
    // spawn 3 - 10 rocks?

    private void Awake()
    {
        SpawnRocks();
    }

    public void SpawnRocks()
    {
        int amount = Random.Range(1, 9);

        for (int i = 0; i < amount; i++)
        {
            float scale = Random.Range(100F, 300F);
            float angle = Random.Range(0F, 360F);
            Vector2 point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * Random.Range(10F, 40F);

            var rock = Instantiate(rockVariants[Random.Range(0, rockVariants.Count)], new Vector3(point.x, 0F, point.y), Quaternion.Euler(Vector3.zero));
            rock.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
