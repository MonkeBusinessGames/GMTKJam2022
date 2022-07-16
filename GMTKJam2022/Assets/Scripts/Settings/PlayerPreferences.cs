using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour
{
    [SerializeField]
    Slider volumeSlider;

    public AudioClip testSFX;



    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("ListenerVolume");

        GetComponent<AudioSource>().PlayOneShot(testSFX);
    }

    public void SetListenerVolumeValue()
    {
        //AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("ListenerVolume", volumeSlider.value);
    }
    
}
