using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeChunkLogic : MonoBehaviour
{
    [SerializeField]
    Transform endPoint;
    [SerializeField]
    GameObject rootObject;

    public Transform EndtPoint { get => endPoint; }
    public GameObject RootObject { get => rootObject; }

    public float Length { get => Vector3.Distance(transform.position, endPoint.position); }
}
