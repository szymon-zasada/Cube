using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretBehaviour : MonoBehaviour
{

    [Header("Scriptable turret base stats")]
    [SerializeField] Turret scriptableTurret;

    [SerializeField] Turret turrett;
    public Turret turret => turrett;

    [Header("Turret component scripts")]
    [SerializeField] GameObject turretHead;
    [SerializeField] List<ProjectileHandler> listOfProjectileHandlers;
    [SerializeField] List<LaserHandler> listOfLaserHandlers;
    [SerializeField] ProjectileHandler projectileHandler;
    [SerializeField] ProjectileHandler projectileHandler2;
    [SerializeField] LaserHandler laserHandler;
    [SerializeField] HealthBarScript healthBarScript;

    [Header("Audio")]
    [SerializeField] AudioClip playerDamaged;
    [SerializeField] AudioClip playerDestroyed;


    [Header("Particles")]
    [SerializeField] ParticleSystem deathExplosion;
    [SerializeField] ParticleSystem levelUpEffect;

    [Header("Animators")]
    [SerializeField] Animator animatorSingle;
    [SerializeField] Animator animatorDouble;
    [SerializeField] Animator animatorSpecial;

    [SerializeField] ShopUnlock possibleUpgrade, possibleUpgrade2;
    public ShopUnlock PossibleUpgrade => possibleUpgrade;
    public ShopUnlock PossibleUpgrade2 => possibleUpgrade2;
    public GameObject TurretHead => turretHead;

    [Header("Variables")]
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Vector3? targetPositionNullable;
    [SerializeField] bool isReloading;
    [SerializeField] bool isReloading2;
    [SerializeField] bool isDead;
    GameManager gameManager;
    public bool IsDead => isDead;


    //F________________________________________________________________________________________________________________________________________________________

    #region Start
    void Start()
    {
        turrett = Turret.CreateInstance(scriptableTurret);
        turrett.TurretBehaviour = this;
        gameManager = GameManager.Instance;
        GameManager.Instance.AddTurret(transform);
    }
    bool alreadyUpgraded = false;
    #endregion

    public void Update()
    {
        if (isDead)
        {
            animatorSingle.SetBool("isReloading", false);
            SetLasersOff();
            return;
        }




        AnimatorHandler();
        HealthRegen();

        if (turret.Type == TurretType.orbital)
        {
            AutoRotate();
            SpawnOrbs();
            return;
        }

        if (turret.Type == TurretType.autoLaser ||
            turret.Type == TurretType.autoProjectile)
            AutoRotateIdle();

        targetPositionNullable = gameManager.GetClosestEntity(gameObject.transform.position, turret.FireRange, gameManager.Enemies);

        if (!targetPositionNullable.HasValue)
        {
            SetLasersOff();
            return;
        }



        // if (((Vector3)targetPositionNullable).sqrMagnitude > turret.FireRange)
        // {
        //     SetLasersOff();
        //     return;
        // }




        targetPosition = (Vector3)targetPositionNullable;

        if (turret.Type == TurretType.autoProjectile)
        {
            AutoRotate();
            AutoShoot();
            return;
        }

        if (turret.Type == TurretType.autoLaser)
            AutoRotate();
        else
            RotateTowardsClosestEnemy();

        AttackingSequence();

    }


    #region ORBITALTURRET
    [SerializeField] List<Bullet> listOfOrbitalBullets = new List<Bullet>();

    void SpawnOrbs()
    {
        int amount = (int)(turret.FireRate / 2f) + 1;

        if (amount == listOfOrbitalBullets.Count)
            return;

        ClearOrbitalBulletList();


        for (int i = 0; i < amount; i++)
        {
            float angle = i * Mathf.PI * 2f / amount;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * turret.FireRange, 0f, Mathf.Sin(angle) * turret.FireRange);
            listOfOrbitalBullets.Add(projectileHandler.SpawnOrbitalBullet(turret, isCritical(), newPos));
            listOfOrbitalBullets[i].gameObject.transform.SetParent(TurretHead.transform);
        }

    }


    public void AddRangeOrbital()
    {
        int amount = (int)(turret.FireRate / 2f) + 1;
        ClearOrbitalBulletList();

        for (int i = 0; i < amount; i++)
        {
            float angle = i * Mathf.PI * 2f / amount;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * turret.FireRange, 0f, Mathf.Sin(angle) * turret.FireRange);
            listOfOrbitalBullets.Add(projectileHandler.SpawnOrbitalBullet(turret, isCritical(), newPos));
            listOfOrbitalBullets[i].gameObject.transform.SetParent(TurretHead.transform);
        }
    }

    void ClearOrbitalBulletList()
    {
        if (listOfOrbitalBullets.Count > 0)
        {
            foreach (Bullet bullet in listOfOrbitalBullets)
            {

                bullet.gameObject.transform.SetParent(null, true);
                bullet.gameObject.SetActive(false);
            }
            listOfOrbitalBullets.Clear();
        }
    }


    public void UpdateOrbs(Bullet bullet)
    {
        listOfOrbitalBullets.Find(b => b == bullet).ChangeCrit(isCritical());
    }
    #endregion

    #region AUTOTURRET
    public void AutoRotate()
    {
        float singleStep = turret.RotationSpeed * Time.deltaTime;
        Vector3 rotateVector = Vector3.RotateTowards(turretHead.transform.forward, turretHead.transform.right, singleStep, 0.0f);
        turretHead.transform.rotation = Quaternion.LookRotation(rotateVector);
    }

    public void AutoRotateIdle()
    {
        float singleStep = turret.RotationSpeed / 2f * Time.deltaTime;
        Vector3 rotateVector = Vector3.RotateTowards(turretHead.transform.forward, turretHead.transform.right, singleStep, 0.0f);
        turretHead.transform.rotation = Quaternion.LookRotation(rotateVector);
    }

    public void AutoShoot()
    {
        if (turret.Type == TurretType.laser)
            laserHandler.SetLaserOn(Vector3.Distance(turretHead.transform.position, targetPosition) * 1.4f);

        if (isReloading)
            return;

        MultipleShot(isCritical());
    }
    #endregion

    #region UPGRADES
    public bool UpgradeTurret()
    {
        if (gameManager.CrystalAmount < possibleUpgrade.UpgradeCostInGame)
            return false;



        if (alreadyUpgraded)
            return false;

        gameManager.CrystalAmount -= possibleUpgrade.UpgradeCostInGame;

        alreadyUpgraded = true;


        GameObject newTurret = Instantiate(possibleUpgrade.TurretPrefab, transform.position, transform.rotation);
        gameManager.RemoveUpgradedTurret(transform);
        Destroy(gameObject);
        return true;
    }

    public bool SecondUpgradeTurret()
    {
        if (gameManager.CrystalAmount < possibleUpgrade2.UpgradeCostInGame)
            return false;


        if (alreadyUpgraded)
            return false;

        gameManager.CrystalAmount -= possibleUpgrade2.UpgradeCostInGame;

        alreadyUpgraded = true;


        GameObject newTurret = Instantiate(possibleUpgrade2.TurretPrefab, transform.position, transform.rotation);
        gameManager.RemoveUpgradedTurret(transform);
        Destroy(gameObject);
        return true;
    }
    #endregion


    void HealthRegen()
    {
        if (turret.Health >= turret.MaxHealth)
            return;

        if (!turret.HealthRegenTick_Cooldown)
        {
            turret.HealthRegenTick();
            healthBarScript.HealthRegenHpBar(turret.Health, turret.MaxHealth);
        }
    }
    public HealthBarScript HealthBarScript => healthBarScript;

    #region ANIMATION
    void AnimatorHandler()
    {
        if (turret.Type == TurretType.laser)
            return;

        if (turret.Type == TurretType.orbital)
            return;

        if (animatorSpecial != null)
        {
            animatorSpecial.SetBool("isReloading", isReloading);
            animatorSpecial.SetFloat("reloadSpeed", turret.FireRate);
        }

        if (turret.HeadType == TurretTier.MultipleBarrel)
        {
            return;
        }

        animatorSingle.SetBool("isReloading", isReloading);
        animatorSingle.SetFloat("reloadSpeed", turret.FireRate);

        if (turret.HeadType == TurretTier.DoubleBarrel)
        {
            animatorDouble.SetBool("isReloading", isReloading2);
            animatorDouble.SetFloat("reloadSpeed", turret.FireRate);
        }
    }
    #endregion


    void RotateTowardsClosestEnemy()
    {
        Vector3 targetDirection = targetPosition - gameObject.transform.position;
        float singleStep = turret.RotationSpeed * Time.deltaTime;
        Vector3 rotateVector = Vector3.RotateTowards(turretHead.transform.forward, targetDirection, singleStep, 0.0f);
        rotateVector.y = 0f;
        turretHead.transform.rotation = Quaternion.LookRotation(rotateVector);
    }


    bool EnemyInline() //! HEAVY PERFORMANCE
    {
        if (Vector3.Dot((new Vector3(targetPosition.x, 0, targetPosition.z) - turretHead.transform.position).normalized, turretHead.transform.forward) < 0.98f)
            return false;

        return true;
    }

    bool EnemyInLineLight() //F             TESTING IN PROGRESS             ####################
    {
        Vector3 target = (targetPosition - turretHead.transform.position).normalized;
        float dotProduct = (target.x * turretHead.transform.position.x) + (target.y * turretHead.transform.position.y) + (target.z * turretHead.transform.position.z);
        if (dotProduct < 0.98f)
            return false;

        return true;
    }

    void SetLasersOff()
    {
        if (turret.Type == TurretType.laser || turret.Type == TurretType.autoLaser)
        {
            if (turret.HeadType == TurretTier.SingleBarrel)
                laserHandler.SetLaserOff();

            else if (turret.HeadType == TurretTier.MultipleBarrel)
                foreach (LaserHandler handler in listOfLaserHandlers)
                    handler.SetLaserOff();

        }
    }

    void AttackingSequence()
    {
        if (!EnemyInline() && turret.Type != TurretType.autoLaser)
        {
            SetLasersOff();
            return;
        }


        if (isReloading)
            return;
        if (isReloading2)
            return;

        if (turret.Type == TurretType.projectile)
        {
            if (turret.HeadType == TurretTier.SingleBarrel)
            {
                ProjectileShot(isCritical());
                return;
            }

            if (turret.HeadType == TurretTier.MultipleBarrel)
            {
                MultipleShot(isCritical());
                return;
            }

            DoubleProjectileShot(isCritical());
            return;
        }


        if (turret.Type == TurretType.laser)
        {
            laserHandler.SetLaserOn(Vector3.Distance(turretHead.transform.position, targetPosition) * 1.4f);
            LaserShot();
            return;
        }



        if (turret.Type == TurretType.autoLaser)
        {
            MultipleLaserShot(isCritical());
            return;
        }




    }


    bool isCritical()
    {
        float r = Random.Range(0, 100);
        if (r < turret.CritRate)
            return true;

        return false;
    }

    #region SHOOTING

    async void MultipleShot(bool isCritical)
    {
        foreach (ProjectileHandler handler in listOfProjectileHandlers)
        {
            handler.SpawnBullet(turret, isCritical);
        }

        isReloading = true;
        await Task.Delay((int)(1000 / turret.FireRate));
        isReloading = false;
    }

    async void ProjectileShot(bool isCritical)
    {
        projectileHandler.SpawnBullet(turret, isCritical);

        isReloading = true;
        await Task.Delay((int)(1000 / turret.FireRate));
        isReloading = false;
    }

    async void DoubleProjectileShot(bool isCritical)
    {
        isReloading = true;
        projectileHandler.SpawnBullet(turret, isCritical);
        await Task.Delay((int)(1000 / turret.FireRate));
        isReloading = false;

        isReloading2 = true;
        projectileHandler2.SpawnBullet(turret, isCritical);
        await Task.Delay((int)(1000 / turret.FireRate));
        isReloading2 = false;
    }

    async void LaserShot()
    {
        isReloading = true;
        laserHandler.SetStats(turret, isCritical());
        await Task.Delay((int)(1000 / turret.FireRate));
        isReloading = false;
    }


    async void MultipleLaserShot(bool isCritical)
    {
        isReloading = true;
        foreach (LaserHandler handler in listOfLaserHandlers)
        {
            handler.SetStats(turret, isCritical);
            handler.SetLaserOn(turret.FireRange);
        }


        await Task.Delay((int)(1000 / turret.FireRate));
        isReloading = false;
    }

    #endregion


    #region TAKING DAMAGE
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        SoundManager.Instance.PlaySound(playerDamaged);
        turret.TakeDamage(damage);

        if (turret.Health <= 0)
            DeathSequence(20);

        healthBarScript.ChangeHpBar(turret.Health, turret.MaxHealth);
    }

    async void DeathSequence(float timer)
    {
        if (turret.Type == TurretType.orbital)
            ClearOrbitalBulletList();


        SoundManager.Instance.PlaySound(playerDestroyed);
        deathExplosion.Play();
        healthBarScript.gameObject.SetActive(false);
        isDead = true;
        gameManager.KillTurret(transform);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            await Task.Yield();
        }

        Destroy(gameObject);
    }
    #endregion

    public void PlayLevelUp() => levelUpEffect.Play();

    [SerializeField] GameObject selected;
    public void ChangeActive(bool x)
    {
        selected.SetActive(x);
    }
}


