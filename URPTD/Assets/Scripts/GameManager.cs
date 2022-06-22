using System.Numerics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Jobs;
using Unity.Collections;
using Vector3 = UnityEngine.Vector3;
using Cinemachine;


public class GameManager : MonoBehaviour
{

    public GameStats gameStats;
    [SerializeField] List<Transform> turrets = new List<Transform>();
    public CameraPoint CameraPoint;

    [SerializeField] List<Transform> enemies = new List<Transform>();

    public List<Transform> Turrets => turrets;
    public List<Transform> Enemies => enemies;

    [SerializeField] SpawningManager spawningManager;
    public SpawningManager SpawningManager => spawningManager;
    [SerializeField] GameUIManager gameUIManager;
    public GameUIManager GameUIManager => gameUIManager;

    [SerializeField] GameObject turretSpawnArea;
    public GameObject TurretSpawnArea => turretSpawnArea;

    //V________________________________________________________________________________________________________________________________________________________

    [SerializeField] int stage = 0, wave = 0;
    public int Stage => stage;
    public int Wave => wave;
    public void AddWave(int x)
    {
        wave += x;
        if(wave >= 11)
        {
            stage++;
            wave = 1;
        }
        gameUIManager.SetStageWave(stage, wave);
    }


    [SerializeField] float difficulty = 0.0f;
    public float Difficulty => difficulty;
    public void AddDifficulty(float f) => difficulty += f;

    public int CrystalAmount = 0;
    public int CashAmount = 0;
    public int ScoreAmount = 0;

    public void AddExpToAll(int x)
    {
        foreach (Transform t in Turrets)
            t.gameObject.GetComponent<TurretBehaviour>().turret.AddExp(x);
    }


    void Start() => SaveSys.LOAD_INTO(ref gameStats);


    public void AddTurret(Transform x)
    {
        turrets.Add(x);
    }

    public void RemoveUpgradedTurret(Transform x) => turrets.Remove(x);
    public void KillTurret(Transform x)
    {
        turrets.Remove(x);
        if (turrets.Count == 0)
            EndGameHandle();
    }
    public void AddEnemy(Transform x) => enemies.Add(x);
    public void KillEnemy(Transform x) => enemies.Remove(x);



    //* no jobs version
    public Vector3? GetClosestEntity(Vector3 target, float range, List<Transform> enemies)
    {
        float allocatedDistance;
        float closestDistance = Mathf.Infinity;
        Vector3? closest = null;
        foreach (Transform enemy in enemies)
        {
            allocatedDistance = (target - enemy.position).sqrMagnitude;
            if (allocatedDistance < closestDistance && allocatedDistance < range)
            {
                closestDistance = allocatedDistance;
                closest = enemy.position;
            }
        }
        return closest;
    }




    public string FormatNumber(long num)
    {
        long i = (long)Mathf.Pow(10, (int)Mathf.Max(0, Mathf.Log10(num) - 2));

        if (i == 0)
            return "0";

        num = num / i * i;


        if (num >= 1000000000)
            return (num / 1000000000D).ToString("0.##") + "B";
        if (num >= 1000000)
            return (num / 1000000D).ToString("0.##") + "M";
        if (num >= 10000)
            return (num / 1000D).ToString("0#") + "K";
        if (num >= 1000)
            return (num / 1000D).ToString("0.#") + "K";

        return num.ToString("#,0");
    }

    public void EnemyDeath(int cashValue, bool hasCrystal)
    {
        GameManager.Instance.CashAmount += cashValue;
        if (hasCrystal)
        {
            GameManager.Instance.CrystalAmount += 1;
            gameUIManager.CrystalIconAnimator.SetTrigger("getCrystal");
        }
        GameManager.Instance.ScoreAmount += 1;
        gameUIManager.CashIconAnimator.SetTrigger("getCash");
    }




    public void EndGameHandle()
    {
        gameUIManager.EndGameHandle(ScoreAmount);
        Time.timeScale = 0.2f;
    }



    //* JOBS version 
    public Vector3? FindClosestEntityJob(GameObject target, List<GameObject> enemies)
    {
        NativeArray<Vector3> enemiesNative = new NativeArray<Vector3>(enemies.Count, Allocator.TempJob);
        NativeArray<Vector3> result = new NativeArray<Vector3>(1, Allocator.TempJob);
        for (int i = 0; i < enemies.Count; i++)
            enemiesNative[i] = enemies[i].transform.position;

        FindClosestEntityJob job = new FindClosestEntityJob
        {
            closestDistance = Mathf.Infinity,
            enemyPositions = enemiesNative,
            targetPosition = target.transform.position,
            closest = result
        };

        // job.Initialize(enemiesNative, target.transform.position, result);
        JobHandle jobHandle = job.Schedule();
        jobHandle.Complete();


        Vector3? closest = job.closest[0];
        if (closest == Vector3.zero)
            closest = null;


        enemiesNative.Dispose();
        result.Dispose();

        return closest;
    }

    #region Singleton
    public static GameManager Instance { get; private set; }

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


public struct FindClosestEntityJob : IJob
{
    [ReadOnly] public NativeArray<Vector3> enemyPositions;
    [ReadOnly] public Vector3 targetPosition;
    public float closestDistance;
    public float allocatedDistance;

    public NativeArray<Vector3> closest;


    public void Execute()
    {
        foreach (Vector3 enemyPosition in enemyPositions)
        {
            allocatedDistance = enemyPosition.sqrMagnitude;
            if (allocatedDistance < closestDistance)
            {
                closestDistance = allocatedDistance;
                closest[0] = enemyPosition;
            }
        }

    }






}
