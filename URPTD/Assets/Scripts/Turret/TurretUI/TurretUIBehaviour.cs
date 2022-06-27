using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUIBehaviour : MonoBehaviour
{
    [SerializeField] CameraPoint cameraPoint;
    GameUIManager gameUIManager;
    [SerializeField] TurretBehaviour turretBehaviour;

    void Start()
    {
        gameUIManager = GameObject.Find("GameUI").GetComponent<GameUIManager>();
        cameraPoint = GameObject.Find("CameraPoint").GetComponent<CameraPoint>();
    }

    void OnMouseDown()
    {
        cameraPoint.SetPosition(transform.position);
        GameManager.Instance.CameraPoint.SetPosition(transform.position);
        gameUIManager.SetUITarget(turretBehaviour);
    }
}
