using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int enemyCount = 0;

    private void Update()
    {
        if (enemyCount <= 0)
            GameOver();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        FindObjectOfType<Canvas>(true).gameObject.SetActive(true);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}
