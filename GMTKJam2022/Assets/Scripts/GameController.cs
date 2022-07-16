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
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text currentMode;
    [SerializeField] private TMP_Text backupMode;
    private bool paused = false;

    private void Update()
    {
/*        if (enemyCount <= 0)
            GameOver();*/

        if (Input.GetButtonDown("Submit"))
            TogglePause();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
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
        SceneManager.LoadScene(0);
    }

    public void UpdateHealth(float damageDealt)
    {
        healthBar.value -= damageDealt;
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
}
