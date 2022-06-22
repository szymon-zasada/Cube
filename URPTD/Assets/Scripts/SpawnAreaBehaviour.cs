using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnAreaBehaviour : MonoBehaviour
{

    GameObject turretPrefab, cameraPoint;
    int expCost;


    void Start()
    {
        cameraPoint = GameObject.Find("CameraPoint");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            cameraPoint.transform.position = transform.position;
    }


    void OnMouseDown()
    {
        

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit, 100f))
        {
            GameObject x = Instantiate(turretPrefab, hit.point, Quaternion.identity) as GameObject;
            gameObject.SetActive(false);
            GameManager.Instance.CrystalAmount -= expCost;
        }

        gameObject.SetActive(false);
    }


    public void SetPrefab(GameObject prefab, int cost)
    {
        turretPrefab = prefab;
        expCost = cost;
    }


    




}
