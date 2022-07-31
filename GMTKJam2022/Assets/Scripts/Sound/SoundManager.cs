using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;


    [Header("Sources")]
    [SerializeField] private AudioSource gunSource;
    [SerializeField] private AudioSource enemySource;

    [Header("Gun SFX")]
    [SerializeField] private AudioClip sfx_machineGunLoop;
    [SerializeField] private AudioClip sfx_machineGunTail;
    [SerializeField] private AudioClip[] sfx_laserGun;
    [SerializeField] private AudioClip[] sfx_sniper;
    [SerializeField] private AudioClip[] sfx_shotgun;
    [SerializeField] private AudioClip[] sfx_assaultRifle;
    [SerializeField] private AudioClip[] sfx_grenadeLauncher;

    [Header("Enemy SFX")]
    [SerializeField] private AudioClip[] sfx_EnemyDeath;


    public bool gunLoopIsPlaying;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>Play the sfx based on the given gun parameter</summary>
    /// <param name="gunIndex">0-MG, 1-Sniper, 2-Shotgun, 3-Laser, 4-AR</param>
    public void PlayGunSFX(int gunIndex)
    {
        if (gunIndex != 0 && gunLoopIsPlaying)
        {
            PlayGunTail();
        }
        switch (gunIndex)
        {
            case 0:
                if(gunLoopIsPlaying == false)
                {
                    PlayGunLoop();
                }
                break;
            case 1:
                PlayGunshot(sfx_sniper);
                break;
            case 2:
                PlayGunshot(sfx_shotgun);
                break;
            case 3:
                PlayGunshot(sfx_laserGun);
                break;
            case 4:
                PlayGunshot(sfx_assaultRifle);
                break;
            case 5:
                PlayGunshot(sfx_grenadeLauncher);
                break;
        }
    }

    private void PlayGunshot(AudioClip[] clips)
    {
        gunSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    public void PlayGunLoop()
    {
        gunLoopIsPlaying = true;
        gunSource.clip = sfx_machineGunLoop;
        gunSource.loop = true;
        gunSource.Play();
    }

    public void PlayGunTail()
    {
        gunLoopIsPlaying=false;
        gunSource.loop = false;
        gunSource.clip = sfx_machineGunTail;
        gunSource.PlayOneShot(sfx_machineGunTail, 0.5f);
    }

    public void PlayEnemyDeath()
    {
        enemySource.PlayOneShot(sfx_EnemyDeath[Random.Range(0, 2)]);
    }
}
