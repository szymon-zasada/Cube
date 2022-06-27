using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRay : MonoBehaviour
{
    Turret turret;
    bool isActive = true;
    protected bool isCritical;
    public void SetStats(Turret turret, bool crit)
    {
        this.turret = turret;
        this.isCritical = crit;
    }


    // Update is called once per frame
    void Update()
    {
        if(!isActive)
            gameObject.SetActive(false);

        if (transform.position.sqrMagnitude > 900f)
            gameObject.SetActive(false);

        transform.position += transform.forward * turret.FireRate * 8f * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider c)
    {
        if(!isActive)
            return;

        if (c.gameObject.tag != "Enemy")
            return;

        EnemyBehaviour enemyBehaviour = c.gameObject.GetComponent<EnemyBehaviour>();

        if (enemyBehaviour.IsDead)
            return;

        if (isCritical)
            enemyBehaviour.TakeDamage(turret, turret.Damage + (turret.Damage * turret.CritDamage / 100), isCritical, TurretType.projectile);
        else
            enemyBehaviour.TakeDamage(turret, turret.Damage, isCritical, TurretType.projectile);

        isActive = false;

        gameObject.SetActive(false);
    }
}
