using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct DeadBodyPart
{
    public Vector3 position;
    public Quaternion rotation;

    public DeadBodyPart(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}

public class EnemyDeadBodyScript : MonoBehaviour
{
    List<DeadBodyPart> deadBodyParts = new List<DeadBodyPart>();
    void Start()
    {
        foreach (Transform child in transform)
        {
            deadBodyParts.Add(new DeadBodyPart(child.localPosition, child.localRotation));
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < deadBodyParts.Count; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = deadBodyParts[i].position;
            child.localRotation = deadBodyParts[i].rotation;
        }
    }
}
