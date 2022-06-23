using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;

public class SpawningManager : MonoBehaviour
{
    [SerializeField] float spawnRadius = 1f, bufforRadius = 1f;
    [SerializeField] GameObject enemyPrefab;

    bool tickCooldown = false, spawningSequence = false;
    public bool SpawningSequence => spawningSequence;
    public PoolsManager poolsManager => PoolsManager.Instance;
    public GameManager gameManager => GameManager.Instance;

    public void StartWave()
    {
        if (spawningSequence && tickCooldown)
            return;

        gameManager.AddWave(1);

        gameManager.AddDifficulty(0.04f);


        int tickAmount = (gameManager.Wave * 2);
        float tickTime = 8 - (gameManager.Wave / 3);

        SpawnSequence(tickAmount, tickTime);
    }


    async void SpawnSequence(int tickAmount, float tickTime)
    {

        spawningSequence = true;

        while (tickAmount > 0)
        {
            if (!tickCooldown)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
                    return;

                tickAmount--;
                SpawnTick(tickTime);
            }

            await Task.Yield();
        }

        spawningSequence = false;

    }


    async void SpawnTick(float tickTime)
    {
        float minValue = 1 + gameManager.Wave * (gameManager.Turrets.Count * 0.8f);
        float maxValue = minValue * 1.4f;


        SpawnEntity(enemyPrefab, (int)Random.Range(minValue, maxValue));
        tickCooldown = true;
        while (tickTime > 0)
        {
            tickTime -= Time.deltaTime;
            await Task.Yield();
        }

        if (gameManager.Wave == 10)
        {
            SpawnBoss();
        }
        tickCooldown = false;
    }


    void SpawnBoss()
    {
        Vector3 spawnPos = Vector3.zero;
        while (spawnPos == Vector3.zero)
        {
            if (RandomSpawnPoint(gameObject.transform.position, spawnRadius, out spawnPos))
            {
                poolsManager.SpawnEnemy(spawnPos, true);
            }
        }
    }


    void SpawnEntity(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = Vector3.zero;
            while (spawnPos == Vector3.zero)
            {
                if (RandomSpawnPoint(gameObject.transform.position, spawnRadius, out spawnPos))
                {
                    poolsManager.SpawnEnemy(spawnPos, false);
                }
            }
        }
    }


    bool RandomSpawnPoint(Vector3 center, float range, out Vector3 result)
    {
        result = Vector3.zero;

        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        if (Vector3.Distance(randomPoint, Vector3.zero) < bufforRadius)
            return false;


        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        return false;
    }
}
