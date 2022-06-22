using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedScript : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(new UnityEngine.Vector3(0, 1, 0) * Time.deltaTime * 320);
    }
}
