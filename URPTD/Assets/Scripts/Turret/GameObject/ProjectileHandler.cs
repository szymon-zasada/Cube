using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] AudioClip enterSound;


    PoolsManager poolsManager;

    void Start()
    {
        poolsManager = PoolsManager.Instance;
    }

    public void SpawnBullet(Turret turret, bool isCritical)
    {
        poolsManager.SpawnBullet(turret, transform.position, transform.rotation, isCritical);
    }

    public Bullet SpawnOrbitalBullet(Turret turret, bool isCritical, Vector3 spawnPosition) =>
        poolsManager.SpawnOrbitalBullet(turret, spawnPosition, transform.rotation, isCritical);















}
