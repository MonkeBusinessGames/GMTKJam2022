using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour
{
    [SerializeField]
    Slider volumeSlider;




    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("ListenerVolume");
    }

    public void SetListenerVolumeValue()
    {
        //AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("ListenerVolume", volumeSlider.value);
    }
    
}
