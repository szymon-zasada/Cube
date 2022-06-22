using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] bool isCritical, tickCooldown;
    [SerializeField] Turret turret;

    public void SetStats(Turret turret, bool isCritical)
    {
        this.turret = turret;
        this.isCritical = isCritical;
        tickCooldown = false;
    }

    void OnTriggerStay(Collider c)
    {
        if (tickCooldown)
            return;

        if (c.gameObject.tag != "Enemy")
            return;

        EnemyBehaviour enemyBehaviour = c.gameObject.GetComponent<EnemyBehaviour>();

        if (enemyBehaviour.IsDead)
            return;

        if (isCritical)
            enemyBehaviour.TakeDamage(turret, turret.Damage + (turret.Damage * turret.CritDamage / 100), isCritical, TurretType.laser);
        else
            enemyBehaviour.TakeDamage(turret, turret.Damage, isCritical, TurretType.laser);
            
        tickCooldown = true;
    }
}
