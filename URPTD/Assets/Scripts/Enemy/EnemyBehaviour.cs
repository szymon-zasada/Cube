using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;


public class EnemyBehaviour : MonoBehaviour
{
    //v________________________________________________________________________________________________________________________________________________________
    [SerializeField] NavMeshAgent agent;
    [SerializeField] EnemyUI enemyUI;
    [SerializeField] EnemyGenerator enemyGenerator;
    [SerializeField] AudioClip damageTakenProjectile, damageTakenCritProjectile, damageTakenLaser, damageTakenCritLaser;
    [SerializeField] Vector3? targetPositionNullable;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float pickUpChance = 1f;
    GameManager gameManager = GameManager.Instance;
    public bool IsBoss = false;

    public bool IsDead => enemyGenerator.IsDead;

    //V________________________________________________________________________________________________________________________________________________________
    public Enemy Enemy;

    //F________________________________________________________________________________________________________________________________________________________


    void Start()
    {
        gameManager = GameManager.Instance;

    }

    void OnEnable()
    {
        if (IsBoss)
            Enemy = enemyGenerator.GenerateBossStats();
        else
            Enemy = enemyGenerator.GenerateRandomStats();

        agent.speed = Enemy.Speed;
        gameManager.AddEnemy(transform);


        targetPositionNullable = gameManager.GetClosestEntity(gameObject.transform.position, Mathf.Infinity, gameManager.Turrets);
        if (targetPositionNullable.HasValue)
        {
            targetPosition = (Vector3)targetPositionNullable;
            agent.destination = targetPosition;
        }

        enemyGenerator.IsDead = false;
    }

    void Update()
    {
        if (enemyGenerator.IsDead)
            return;

        if (targetPosition == null)
        {

            targetPositionNullable = gameManager.GetClosestEntity(transform.position, Mathf.Infinity, gameManager.Turrets);
            if (targetPositionNullable.HasValue)
                targetPosition = (Vector3)targetPositionNullable;
            agent.destination = targetPosition;
        }
    }

    public void TakeDamage(Turret turret, float damageValue, bool isCritical, TurretType type)
    {
        if (enemyGenerator.IsDead)
            return;


        if (type == TurretType.projectile)
        {
            if (isCritical)
                SoundManager.Instance.PlaySound(damageTakenCritProjectile);
            else
                SoundManager.Instance.PlaySound(damageTakenProjectile);
        }
        else if (type == TurretType.laser)
        {
            if (isCritical)
                SoundManager.Instance.PlaySound(damageTakenCritLaser);
            else
                SoundManager.Instance.PlaySound(damageTakenLaser);
        }



        Enemy.TakeDamage(damageValue);
        enemyUI.DamagePopUp(damageValue, isCritical);

        if (Enemy.Health <= 0f)
        {
            PickUpHandle();
            agent.destination = transform.position;
            gameManager.KillEnemy(transform);
            turret.AddExp(Enemy.CashValue);
            enemyGenerator.IsDead = true;
            gameManager.EnemyDeath(Enemy.CashValue, Enemy.HasCrystal);
            enemyGenerator.DeathTransition();
        }
    }

    void PickUpHandle()
    {
        if (Random.Range(0, 100f) < pickUpChance)
            PoolsManager.Instance.SpawnPickUp(transform.position, transform.rotation);
    }





    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag != "Turret")
            return;
        TurretBehaviour turretBehaviour = c.gameObject.GetComponent<TurretBehaviour>();
        if (turretBehaviour.IsDead)
            return;

        turretBehaviour.TakeDamage(Enemy.Damage);
        turretBehaviour.turret.AddExp(Enemy.CashValue / 4);
        gameManager.KillEnemy(transform);
        gameObject.SetActive(false);
    }

}


