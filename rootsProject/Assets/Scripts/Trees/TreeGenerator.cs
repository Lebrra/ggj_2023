using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;

public class TreeGenerator : MonoBehaviour
{
    public static TreeGenerator instance;

    [SerializeField]
    float growTime = 0.1F;
    float currentSpeed = 0.1F;
    [SerializeField]
    float inputSensitivity = 0.05F;

    Transform returnPoint;
    [SerializeField]
    Transform spawnpoint;
    [SerializeField]
    TreeChunkLogic partPrefab;

    [SerializeField]
    Transform hoverUI;
    [SerializeField]
    GameObject submergedUI;

    Routine growRoot;

    [SerializeField]
    float submergeCooldown = 3F;
    bool submerged = false;
    Routine submergeDelay;

    [SerializeField]
    float resetCooldown = 3F;
    Routine restarted;

    private void Awake()
    {
        if (instance) Destroy(instance);
        instance = this;
    }

    private void Start()
    {
        returnPoint = spawnpoint;
    }

    private void FixedUpdate()
    {
        currentSpeed = growTime * 0.7F * GameStateManager.CurrentNormalizedGameTime + 0.04F;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")) >= inputSensitivity)
        {
            if (submerged)
            {
                // move the identifier
                submergedUI.transform.position += (new Vector3(Input.GetAxis("Horizontal"), 0F, Input.GetAxis("Vertical")) * currentSpeed);
                hoverUI.position = new Vector3(submergedUI.transform.position.x, hoverUI.position.y, submergedUI.transform.position.z);
            }
            else if (!growRoot.Exists())
            {
                growRoot.Replace(SpawnRootPart(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))));
                hoverUI.position = new Vector3(spawnpoint.transform.position.x, hoverUI.position.y, spawnpoint.transform.position.z);
            }
        }
        if (Input.GetButton("Dive"))
        {
            if (!submergeDelay.Exists()) submergeDelay.Replace(ToggleSubmerge());
        }
        if (Input.GetButton("NewSpawn"))
        {
            if (!restarted.Exists()) restarted.Replace(ToggleReset());
        }
    }

    IEnumerator SpawnRootPart(Vector2 playerInput)
    {
        // get correct degrees for all quadrants
        var degree = Mathf.Atan(playerInput.y / -playerInput.x) * Mathf.Rad2Deg;
        if (playerInput.x < 0) degree -= 180F;

        // check to see if ahead is clear
        if (!CheckAhead(spawnpoint, playerInput, partPrefab.Length)) yield break;

        // spawn new part based using rotation
        var newPart = Instantiate(partPrefab, spawnpoint.position, Quaternion.Euler(Random.Range(0F, 360F), degree, 0));
        spawnpoint = newPart.EndtPoint;

        // wait to spawn another
        yield return currentSpeed;
    }

    bool CheckAhead(Transform endpoint, Vector2 playerInput, float distance)
    {
        var playerDirection = new Vector3(playerInput.x, 0F, playerInput.y).normalized;

        Debug.DrawRay(endpoint.position, playerDirection * distance, Color.red);
        RaycastHit[] rayhits = Physics.RaycastAll(endpoint.position, playerDirection * distance, distance);

        var chunkLogic = endpoint.gameObject.GetComponentInParent<TreeChunkLogic>();
        foreach (var hit in rayhits)
        {
            if (chunkLogic == null) return false;
            else if (hit.transform.gameObject != chunkLogic.RootObject) return false;
        }
        return true;
    }

    IEnumerator ToggleSubmerge()
    {
        if (submerged)
        {
            submerged = false;
            submergedUI.SetActive(false);

            // check for skewered here

            yield return submergeCooldown;
        }
        else
        {
            submerged = true;
            submergedUI.SetActive(true);
            submergedUI.transform.position = spawnpoint.position;
            spawnpoint = submergedUI.transform;

            yield return 0.2F;
        }
    }

    IEnumerator ToggleReset()
    {
        ResetPos();
        yield return resetCooldown;
    }

    void ResetPos()
    {
        spawnpoint = returnPoint;
        hoverUI.position = new Vector3(spawnpoint.transform.position.x, hoverUI.position.y, spawnpoint.transform.position.z);
    }

    public void CheckForMissing(Transform destroyedRoot)    // use this when a root is destroyed 
    {
        if (destroyedRoot == spawnpoint)
        {
            ResetPos();
            return;
        }

        foreach (var transform in destroyedRoot.GetComponentsInChildren<Transform>())
            if (transform == spawnpoint)
            {
                ResetPos();
                return;
            }
    }
}
