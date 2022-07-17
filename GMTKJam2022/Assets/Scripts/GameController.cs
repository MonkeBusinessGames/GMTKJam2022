using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int enemyCount = 0;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameOverMenuHighScore;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private TMP_Text newHighScore;
    [SerializeField] private TMP_Text oldHighScore;
    [SerializeField] private TMP_Text currentMode;
    [SerializeField] private TMP_Text backupMode;
    [SerializeField] private GameObject switchVisual;
    [SerializeField] private GameObject credit;
    [SerializeField] private TMP_Text creditHighscore;
    public Image reloadImages;
    [HideInInspector] public float reloadAmount;

    private bool paused = false;
    private SaveData data;
    private bool newBest = false;

    private void Awake()
    {
        data = SaveSystem.Load();
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
        currentMode.text = current;
        backupMode.text = backup;
    }
    public void modeUpdate(string current)
    {
        currentMode.text = current;
    }
    public void MenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void RollCredits()
    {
        creditHighscore.text = "Best: $" + data.highScore.ToString();
        credit.SetActive(true);
    }
}
