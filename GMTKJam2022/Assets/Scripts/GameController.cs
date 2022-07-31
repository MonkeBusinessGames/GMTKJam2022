using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameOverMenuHighScore;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private TMP_Text newHighScore;
    [SerializeField] private TMP_Text oldHighScore;
    [SerializeField] private Image currentMode;
    [SerializeField] private Image backupMode;
    [SerializeField] private Sprite shotgun;
    [SerializeField] private Sprite ar;
    [SerializeField] private Sprite sniper;
    [SerializeField] private Sprite laser;
    [SerializeField] private Sprite machineGun;
    [SerializeField] private Sprite grenade;
    [SerializeField] private GameObject switchVisual;
    [SerializeField] private TMP_Text startText;

    private bool paused = false;
    private SaveData data;
    private bool newBest = false;

    private void Awake()
    {
        data = SaveSystem.Load();

    }

    private void Start()
    {
        StartCoroutine(StartWait());
    }

    private void Update()
    {

        if (Input.GetButtonDown("Submit"))
            TogglePause();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        if (newBest)
        {
            SaveSystem.Save(data);
            gameOverMenuHighScore.SetActive(true);
            newHighScore.text = score.text;
            return;
        }
        gameOverMenu.SetActive(true);
        finalScore.text = score.text;
        oldHighScore.text = "Best: $" + data.highScore.ToString();
    }
    public void TogglePause()
    {
        if(paused)
        {
            paused = false;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            return;
        }
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void UpdateHealth(float damageDealt)
    {
        healthBar.value -= damageDealt;
    }

    public void UpdateScore(float money)
    {
        score.text = "$" + money.ToString();
        if (money > data.highScore)
        {
            data.highScore = money;
            newBest = true;
        }
    }
    public IEnumerator ShowSwitchVisual()
    {
        switchVisual.SetActive(true);
        yield return new WaitForSeconds(2f);
        switchVisual.SetActive(false);
    }

    public void modeUpdate(string current, string backup)
    {
        switch (current)
        {
            case "Machine Gun":
                currentMode.sprite = machineGun;
                break;
            case "Sniper":
                currentMode.sprite = sniper;
                break;
            case "Shotgun":
                currentMode.sprite = shotgun;
                break;
            case "Laser":
                currentMode.sprite = laser;
                break;
            case "Grenade":
                currentMode.sprite = grenade;
                break;
            case "Assault Rifle":
                currentMode.sprite = ar;
                break;
        }

        switch (backup)
        {
            case "Machine Gun":
                backupMode.sprite = machineGun;
                break;
            case "Sniper":
                backupMode.sprite = sniper;
                break;
            case "Shotgun":
                backupMode.sprite = shotgun;
                break;
            case "Laser":
                backupMode.sprite = laser;
                break;
            case "Grenade":
                backupMode.sprite = grenade;
                break;
            case "Assault Rifle":
                backupMode.sprite = ar;
                break;
        }
    }
    public void modeUpdate(string current)
    {
        switch (current)
        {
            case "Machine Gun":
                currentMode.sprite = machineGun;
                break;
            case "Sniper":
                currentMode.sprite = sniper;
                break;
            case "Shotgun":
                currentMode.sprite = shotgun;
                break;
            case "Laser":
                currentMode.sprite = laser;
                break;
            case "Grenade":
                currentMode.sprite = grenade;
                break;
            case "Assault Rifle":
                currentMode.sprite = ar;
                break;
        }
    }
    public void MenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator StartWait()
    {
        Time.timeScale = 0;
        print(gameObject.name);
        yield return new WaitForSecondsRealtime(1.5f);
        startText.gameObject.SetActive(true);
        startText.text = "GAMBLE GUN ACQUIRED";

        yield return new WaitForSecondsRealtime(2.5f);

        startText.text = "ENEMIES APPROACHING";

        yield return new WaitForSecondsRealtime(2.5f);

        startText.text = "TIME TO CASH IN";

        yield return new WaitForSecondsRealtime(2.5f);

        startText.text = "GO!";

        yield return new WaitForSecondsRealtime(1.5f);

        startText.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
