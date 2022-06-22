using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    Turret turret;
    [SerializeField] GameObject cannonBall;
    protected float projectileSpeed;
    protected bool isCritical;
    SoundManager soundManager;
    public void ChangeCrit(bool value) => isCritical = value;
    public void Start()
    {
        soundManager = SoundManager.Instance;
    }

    public void SetStats(Turret turret, bool crit)
    {
        this.turret = turret;
        cannonBall.SetActive(false);
        isCritical = crit;
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
