using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [SerializeField] private float timeToExplode;
    [SerializeField] private float explosionRadius;
    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(timeToExplode);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i=0; i<enemies.Length; i++)
        {
            if((enemies[i].transform.position - transform.position).sqrMagnitude < explosionRadius)
            {
                enemies[i].GetComponent<EnemyController>().Damaged(damage);
            }
        }
        Destroy(gameObject);
    }
}
