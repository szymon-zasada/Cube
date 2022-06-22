using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FPSCounter : MonoBehaviour
{
    [SerializeField] TMP_Text fps;


    void Start()
    {
        InvokeRepeating("GetFPS", 0.3f, 0.3f);
    }

    // Update is called once per frame
    void GetFPS()
    {
        fps.text = $"FPS: {(int)(1f / Time.unscaledDeltaTime)}";
    }
}
