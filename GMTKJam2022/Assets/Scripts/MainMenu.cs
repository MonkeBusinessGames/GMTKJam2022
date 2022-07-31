using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject controls;
    [SerializeField] GameObject credits;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider volume;

    private void Start()
    {

        mixer.GetFloat("Volume", out float mixerValue);
        volume.value = Mathf.Exp(mixerValue / 20);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Menu()
    {
        controls.SetActive(false);
        credits.SetActive(false);
    }
    public void Controls()
    {
        controls.SetActive(true);
        credits.SetActive(false);
    }
    public void Credits()
    {
        controls.SetActive(false);
        credits.SetActive(true);
    }

    public void SetVolume(float value)
    {
        mixer.SetFloat("Volume", Mathf.Log(value) * 20);
    }
}
