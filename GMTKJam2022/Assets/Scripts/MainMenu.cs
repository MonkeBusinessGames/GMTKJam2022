using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject controls;
    [SerializeField] GameObject credits;
    [SerializeField] AudioMixer mixer;


    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
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
