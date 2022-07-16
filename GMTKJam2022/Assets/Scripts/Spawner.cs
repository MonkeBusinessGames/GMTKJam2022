using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
}

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Vector2 boundaries;
    private float notSpawnBox=5f;
    int currentWave=0;
    public float waveDelay=2f;
    float waveTimer;
    bool spawn = false;

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0 && !spawn)
        {
            StartCoroutine(Spawn());
        }
    }
    private IEnumerator Spawn()
    {
        spawn = true;
        yield return new WaitForSeconds(waveDelay);
        foreach (GameObject enemy in waves[currentWave].enemies)
        {
            Vector2 spawnPos= getSpawnPos(new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y));
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
        yield return null;
        spawn = false;
        currentWave += 1;
    }

    Vector2 getSpawnPos(Vector2 playerPos)
    {
        Vector2 spawn = playerPos;
        while (spawn.x > playerPos.x - notSpawnBox && spawn.x < playerPos.x + notSpawnBox && spawn.y > playerPos.y - notSpawnBox && spawn.y < playerPos.y + notSpawnBox)
        {
            spawn = new Vector2(Random.Range(-boundaries.x, boundaries.x), Random.Range(-boundaries.y, boundaries.y));
        }
        return spawn;
    }
}
