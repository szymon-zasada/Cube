using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake(){
        GameObject[] musicSources = GameObject.FindGameObjectsWithTag("Music");
        if(musicSources.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
