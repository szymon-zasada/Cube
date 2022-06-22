using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public enum BulletType { normal, sniper, minigun, orbital, railgun }
public static class Bullets
{
    public static List<BulletType> NonDestructible = new List<BulletType> { BulletType.sniper, BulletType.orbital, BulletType.railgun };
}

public class Bullet : MonoBehaviour
{
    EnemyBehaviour lastAttacked;
    [SerializeField] GameObject bullet, sniperBullet, minigunBullet, orbitalBullet, railgunBullet;
    Turret turret;
    protected float projectileSpeed;
    protected bool isCritical;
    [SerializeField] TrailRenderer trailRenderer, sniperTrailRenderer, minigunTrailRenderer, orbitalTrailRenderer, railgunTrailRenderer;
    [SerializeField] ParticleSystem bulletParticle, sniperBulletParticle;
    SoundManager soundManager;
    bool isActive = true;

    public void ChangeCrit(bool value) => isCritical = value;

    public void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    public void Clear()
    {
        trailRenderer.Clear();
        sniperTrailRenderer.Clear();
        minigunTrailRenderer.Clear();
        orbitalTrailRenderer.Clear();
        railgunTrailRenderer.Clear();
    }

    

    public void SetStats(Turret turret, bool crit)
    {
        this.turret = turret;
        bullet.SetActive(false);
        sniperBullet.SetActive(false);
        minigunBullet.SetActive(false);
        orbitalBullet.SetActive(false);
        railgunBullet.SetActive(false);
        isActive = true;

        if (turret.BulletType == BulletType.normal)
        {
            bullet.SetActive(true);
            soundManager.PlaySound(soundManager.BulletEnter);
            projectileSpeed = turret.FireRate * 2f;
        }

        else if (turret.BulletType == BulletType.sniper)
        {
            sniperBullet.SetActive(true);
            soundManager.PlaySound(soundManager.SniperEnter);
            projectileSpeed = turret.FireRate * 20f;
        }

        else if (turret.BulletType == BulletType.minigun)
        {
            minigunBullet.SetActive(true);
            soundManager.PlaySound(soundManager.MinigunEnter);
            projectileSpeed = turret.FireRate * 2f;
        }

        else if (turret.BulletType == BulletType.orbital)
        {
            orbitalBullet.SetActive(true);
            gameObject.transform.SetParent(turret.TurretBehaviour.TurretHead.transform);
            projectileSpeed = 0f;
        }

        else if (turret.BulletType == BulletType.railgun)
        {
            railgunBullet.SetActive(true);
            soundManager.PlaySound(soundManager.RailgunEnter);
            projectileSpeed = turret.FireRate * 35f;
        }



        isCritical = crit;
    }


    void Update()
    {
        if(!isActive)
            return;

        if (turret.BulletType == BulletType.orbital)
            return;

        if (!bullet.activeSelf && !sniperBullet.activeSelf && !minigunBullet.activeSelf && !orbitalBullet.activeSelf && !railgunBullet.activeSelf)
            return;
        if (transform.position.sqrMagnitude > 900f)
            gameObject.SetActive(false);

        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }


    private async void OnTriggerEnter(Collider c)
    {   
        if(!isActive)
            return;

        if (c.gameObject.tag != "Enemy")
            return;

        EnemyBehaviour enemyBehaviour = c.gameObject.GetComponent<EnemyBehaviour>();

        if (enemyBehaviour.IsDead)
            return;


        if (lastAttacked == enemyBehaviour)
            return;

        if (isCritical)
            enemyBehaviour.TakeDamage(turret, turret.Damage + (turret.Damage * turret.CritDamage / 100), isCritical, TurretType.projectile);
        else
            enemyBehaviour.TakeDamage(turret, turret.Damage, isCritical, TurretType.projectile);

        
        if (turret.BulletType == BulletType.normal)
        {
            bullet.SetActive(false);
            bulletParticle.Play();
            isActive = false;
            await Task.Delay(300);
            gameObject.SetActive(false);
        }
        else if(turret.BulletType == BulletType.sniper)
        {
            sniperBulletParticle.Play();
        }
        else if (turret.BulletType == BulletType.minigun)
        {
            minigunBullet.SetActive(false);

            await Task.Delay(300);
            gameObject.SetActive(false);
        }
        else if (turret.BulletType == BulletType.orbital)
        {
            lastAttacked = enemyBehaviour;
            turret.TurretBehaviour.UpdateOrbs(this);
        }



        

    }




}
