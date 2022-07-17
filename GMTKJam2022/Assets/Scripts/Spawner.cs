using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    public Transform[] points;
}

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Vector2 boundaries;
    public GameController gameController;
    int currentWave=0;
    public float waveDelay=2f;
    public float spawnDelay = 2f;
    float waveTimer;
    bool spawn = false;

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0 && !spawn)
        {
            if(currentWave < waves.Length)
            {
                StartCoroutine(Spawn());
            }
            else
            {
                StartCoroutine(Credits());
            }
        }
    }
    private IEnumerator Spawn()
    {
        spawn = true;
        yield return new WaitForSeconds(waveDelay);
        int e = waves[currentWave].enemies.Length-1;
        while (e >= 0)
        {
            for(int p=0; p< waves[currentWave].points.Length; p++)
            {
                Instantiate(waves[currentWave].enemies[e], waves[currentWave].points[p].position, Quaternion.identity);
                e--;
                if (e < 0)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
        }
        currentWave += 1;
        spawn = false;
    }
    private IEnumerator Credits()
    {
        yield return new WaitForSeconds(7.5f);
        gameController.RollCredits();
    }
}
