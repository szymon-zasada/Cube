using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip levelUp, bulletEnter, sniperEnter, railgunEnter, minigunEnter;
    public AudioClip LevelUp => levelUp;
    public AudioClip BulletEnter => bulletEnter;
    public AudioClip SniperEnter => sniperEnter;
    public AudioClip RailgunEnter => railgunEnter;
    public AudioClip MinigunEnter => minigunEnter;
    
    [SerializeField] AudioSource audioComponent;

   
    public AudioSource AudioComponent => audioComponent;

    

    private void Start()
    {
        audioComponent.volume = GameManager.Instance.gameStats.SoundVolume;
    }

    public void PlaySound(AudioClip clip) => audioComponent.PlayOneShot(clip);





    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }
}
