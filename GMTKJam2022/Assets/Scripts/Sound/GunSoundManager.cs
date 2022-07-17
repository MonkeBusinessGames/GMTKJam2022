using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoundManager : MonoBehaviour
{
    public static GunSoundManager Instance;

    [Header("Sound FX")]
    [SerializeField] private AudioClip sfx_machineGunLoop;
    [SerializeField] private AudioClip sfx_machineGunTail;
    [SerializeField] private AudioClip[] sfx_laserGun;
    [SerializeField] private AudioClip[] sfx_shotgun;
    [SerializeField] private AudioClip[] sfx_sniper;
    [SerializeField] private AudioClip[] sfx_assaultRifle;

    AudioSource player_audioSource;

    public bool machineGunSoundIsPlaying;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGunSFX(int gunIndex)
    {
        if (gunIndex != 0 && machineGunSoundIsPlaying)
        {
            PlayGunTail();
        }
        switch (gunIndex)
        {
            case 0:
                if(machineGunSoundIsPlaying == false)
                {
                    playGunLoop();
                }
                break;
            case 1:
                playGunshot(sfx_sniper);
                break;
            case 2:
                playGunshot(sfx_shotgun);
                break;
            case 3:
                playGunshot(sfx_laserGun);
                break;
            case 4:
                playGunshot(sfx_assaultRifle);
                break;
            case 5:
                break;
        }
    }

    private void playGunshot(AudioClip[] clips)
    {
        player_audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    public void playGunLoop()
    {
        machineGunSoundIsPlaying = true;
        player_audioSource.clip = sfx_machineGunLoop;
        player_audioSource.loop = true;
        player_audioSource.Play();
    }

    public void PlayGunTail()
    {
        machineGunSoundIsPlaying=false;
        player_audioSource.loop = false;
        player_audioSource.clip = sfx_machineGunTail;
        player_audioSource.PlayOneShot(sfx_machineGunTail, 0.5f);
    }
}
