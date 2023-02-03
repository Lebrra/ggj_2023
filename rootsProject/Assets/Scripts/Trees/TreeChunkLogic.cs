using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeChunkLogic : MonoBehaviour
{
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    Transform endPoint;

    public Transform StartPoint { get => startPoint; }
    public Transform EndtPoint { get => endPoint; }
}
