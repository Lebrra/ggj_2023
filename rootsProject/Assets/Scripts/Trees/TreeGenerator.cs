using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;

public class TreeGenerator : MonoBehaviour
{
    public static TreeGenerator instance;

    [SerializeField]
    Transform cameraAngle;

    [SerializeField]
    float growTime = 0.1F;
    float currentSpeed = 0.1F;
    [SerializeField]
    float inputSensitivity = 0.05F;

    float currentSize = 1F;
    float time = 0F;

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
        time += Time.deltaTime;
        currentSize = Mathf.Clamp((5F - time) / 10F + 0.8F, 0.6F, 1.2F);

        if (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")) >= inputSensitivity)
        {
            Vector3 playerCameraInput = Quaternion.AngleAxis(cameraAngle.rotation.eulerAngles.y, Vector3.up) * new Vector3(Input.GetAxis("Horizontal"), 0F, Input.GetAxis("Vertical"));

            if (submerged)
            {
                // move the identifier
                Vector3 normal = playerCameraInput * ( 1F - currentSpeed) / 7.5F;
                Vector3 pos = submergedUI.transform.position;
                submergedUI.transform.position = normal + pos;

                hoverUI.position = new Vector3(submergedUI.transform.position.x, hoverUI.position.y, submergedUI.transform.position.z);
            }
            else if (!growRoot.Exists())
            {
                growRoot.Replace(SpawnRootPart(playerCameraInput));
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

    IEnumerator SpawnRootPart(Vector3 playerInput)
    {
        // get correct degrees for all quadrants
        var degree = Mathf.Atan(playerInput.z / -playerInput.x) * Mathf.Rad2Deg;
        if (playerInput.x < 0) degree -= 180F;

        // check to see if ahead is clear
        if (!CheckAhead(spawnpoint, playerInput, partPrefab.Length)) yield break;

        // spawn new part based using rotation
        var newPart = Instantiate(partPrefab, spawnpoint.position, Quaternion.Euler(Random.Range(0F, 360F), degree, 0));
        newPart.transform.SetScale(currentSize);
        spawnpoint = newPart.EndtPoint;

        // wait to spawn another
        yield return currentSpeed;
    }

    bool CheckAhead(Transform endpoint, Vector3 playerInput, float distance)
    {
        var playerDirection = new Vector3(playerInput.x, 0F, playerInput.z).normalized;

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
            CheckForAmbush();

            yield return submergeCooldown;
        }
        else
        {
            var tempPos = spawnpoint.position;
            submergedUI.transform.position = new Vector3(tempPos.x, 0F, tempPos.z);
            spawnpoint = submergedUI.transform;
            submergedUI.SetActive(true);
            submerged = true;

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
        time = 0F;
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

    public void CheckForAmbush()
    {
        // sphere cast on submerge UI, kill all lumberjacks

        RaycastHit[] rayhits = Physics.SphereCastAll(submergedUI.transform.position, 2F, submergedUI.transform.up);
        foreach (var hit in rayhits)
        {
            var ai = hit.transform.GetComponent<LumberJacks>();
            if (ai != null)
            {
                Debug.LogWarning(ai.name + " DIED", ai.transform.gameObject);
                ai.KillMe();
            }
        }
    }
}
