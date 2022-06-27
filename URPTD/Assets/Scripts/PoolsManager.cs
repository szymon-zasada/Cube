using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{


    [Header("Enemies")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int enemyPoolSize;
    [SerializeField] Queue<GameObject> enemyPool;




    [Header("Bullets")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletPoolSize;
    [SerializeField] Queue<BulletStruct> bulletPool;


    [Header("PickUps")]
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] int pickupPoolSize;
    [SerializeField] Queue<GameObject> pickupPool;


    [Header("Lightning")]
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] int lightningSize;
    [SerializeField] Queue<GameObject> lightningPool;


    void Start()
    {

        bulletPool = new Queue<BulletStruct>();
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletStruct b = new BulletStruct(obj, obj.GetComponent<Bullet>());
            obj.SetActive(false);
            bulletPool.Enqueue(b);
        }

        enemyPool = new Queue<GameObject>();
        for (int i = 0; i < enemyPoolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, -transform.position, transform.rotation);
            enemyPool.Enqueue(obj);
        }

        pickupPool = new Queue<GameObject>();
        for (int i = 0; i < pickupPoolSize; i++)
        {
            GameObject obj = Instantiate(pickupPrefab, -transform.position, transform.rotation);
            pickupPool.Enqueue(obj);
        }


        lightningPool = new Queue<GameObject>();
        for (int i = 0; i < lightningSize; i++)
        {
            GameObject obj = Instantiate(lightningPrefab, -transform.position, transform.rotation);
            obj.SetActive(false);
            lightningPool.Enqueue(obj);
        }
    }




    public void SpawnBullet(Turret turret, Vector3 position, Quaternion rotation, bool isCritical)
    {
        BulletStruct objectToSpawn = bulletPool.Dequeue();
        objectToSpawn.gameObjectBody.SetActive(true);
        objectToSpawn.gameObjectBody.transform.position = position;
        objectToSpawn.gameObjectBody.transform.rotation = rotation;
        objectToSpawn.bullet.Clear();
        objectToSpawn.bullet.SetStats(turret, isCritical);

        bulletPool.Enqueue(objectToSpawn);
    }

    public Bullet SpawnOrbitalBullet(Turret turret, Vector3 position, Quaternion rotation, bool isCritical)
    {
        BulletStruct objectToSpawn = bulletPool.Dequeue();
        objectToSpawn.gameObjectBody.SetActive(true);
        objectToSpawn.gameObjectBody.transform.position = position;
        objectToSpawn.gameObjectBody.transform.rotation = rotation;
        objectToSpawn.bullet.Clear();
        objectToSpawn.bullet.SetStats(turret, isCritical);

        bulletPool.Enqueue(objectToSpawn);
        return objectToSpawn.bullet;
    }

    public void SpawnPickUp(Vector3 position, Quaternion rotation)
    {
        GameObject x = pickupPool.Dequeue();
        x.transform.position = position;
        x.SetActive(true);
        pickupPool.Enqueue(x);
    }

    public void SpawnEnemy(Vector3 position, bool IsBoss)
    {
        GameObject x = enemyPool.Dequeue();
        x.transform.position = position;
        x.SetActive(true);
        enemyPool.Enqueue(x);
    }

    public void SpawnLightningRay(Turret turret, Vector3 position, Quaternion rotation, bool isCritical)
    {
        GameObject objectToSpawn = lightningPool.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        LightningRay x = objectToSpawn.GetComponent<LightningRay>();
        x.SetStats(turret, isCritical);

        lightningPool.Enqueue(objectToSpawn);
    }





    #region Singleton 
    public static PoolsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }
    #endregion
}

[System.Serializable]
public struct BulletStruct
{
    public GameObject gameObjectBody;
    public Bullet bullet;
    public BulletStruct(GameObject gameObjectBody, Bullet bullet)
    {
        this.bullet = bullet;
        this.gameObjectBody = gameObjectBody;
    }

}




[System.Serializable]
public struct EnemyStruct
{
    public GameObject gameObject;
    public EnemyBehaviour enemyBehaviour;
    public EnemyStruct(GameObject gameObject, EnemyBehaviour enemyBehaviour)
    {
        this.gameObject = gameObject;
        this.enemyBehaviour = enemyBehaviour;
    }
}