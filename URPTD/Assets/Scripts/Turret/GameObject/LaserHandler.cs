using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab, laserBeam;
    [SerializeField] BoxCollider laserCollider;
    [SerializeField] AudioClip laserSound;
    SoundManager soundManager;
    bool soundCooldown = false;

    Laser laser;
    void Start()
    {
        soundManager = SoundManager.Instance;
        laser = laserPrefab.GetComponent<Laser>();
    }
    public void SetStats(Turret turret, bool isCritical) => laser.SetStats(turret, isCritical);
    




    public async void SetLaserOn(float dist)
    {
        if (!laserPrefab.activeSelf)
        {
            laserPrefab.SetActive(true);
        }




        laserCollider.size = new Vector3(15, 4, dist);
        laserCollider.center = new Vector3(0, 0, dist / 1.8f);
        laserBeam.transform.localScale = new Vector3(0.1f, 0.1f, laserCollider.size.z / 5f);


        if (soundCooldown)
            return;

        soundCooldown = true;
        soundManager.PlaySound(laserSound);
        await Task.Delay(50);
        soundCooldown = false;
    }

    public void SetLaserOff() => laserPrefab.SetActive(false);


}
