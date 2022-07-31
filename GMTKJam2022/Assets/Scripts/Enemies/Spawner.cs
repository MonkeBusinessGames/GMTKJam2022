using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject[] allEnemies;
    public Transform[] allPoints;
    private GameObject[] wave;
    private int waveCounter = 0;
    public Vector2 boundaries;
    public GameController gameController;
    private float waveDelay = 5f;
    private float spawnDelay = 2f;
    bool spawn = false;
    public static int enemyCount = 0;

    void Update()
    {
        if (enemyCount == 0 && !spawn)
        {
            if(waveCounter < 1)
                StartCoroutine(Spawn(new GameObject[] { allEnemies[0] }, new Transform[] { allPoints[0] }));
            else if (waveCounter < 2)
                StartCoroutine(Spawn(new GameObject[] { allEnemies[0] }, new Transform[] { allPoints[0], allPoints[1]}));
            else if (waveCounter < 4)
                StartCoroutine(Spawn(new GameObject[] { allEnemies[0], allEnemies[1] }, new Transform[] { allPoints[0], allPoints[1] }));
            else if (waveCounter < 6)
                StartCoroutine(Spawn(new GameObject[] { allEnemies[0], allEnemies[1] }, new Transform[] { allPoints[0], allPoints[1], allPoints[2] }));
            else if (waveCounter < 8)
                StartCoroutine(Spawn(allEnemies, new Transform[] { allPoints[0], allPoints[1], allPoints[2] }));
            else
                StartCoroutine(Spawn(allEnemies, allPoints));
        }
    }
    private IEnumerator Spawn(GameObject[] enemies, Transform[] points)
    {
        spawn = true;
        //Generate Wave
        wave = new GameObject[5 + 2*waveCounter];

        for(int w = 0; w < wave.Length; w++)
        {
            wave[w] = enemies[Random.Range((int)0, (int)enemies.Length)];
        }

        yield return new WaitForSeconds(waveDelay);
        int e = wave.Length-1;
        while (e >= 0)
        {
            for(int p=0; p< points.Length; p++)
            {
                Instantiate(wave[e], points[p].position, Quaternion.identity);
                enemyCount++;
                e--;
                if (e < 0)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
        }
        waveCounter += 1;
        if (spawnDelay > .5f) 
            spawnDelay -= .2f; 
        spawn = false;
    }
}
