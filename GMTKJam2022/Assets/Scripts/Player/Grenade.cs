using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [SerializeField] private float timeToExplode;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float gunShakeAmpl;
    [SerializeField] private float gunShakeDur;
    [SerializeField] private int damage;
    public Vector2 destination;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    IEnumerator Explode()
    {
        while((Vector2)transform.position != destination)
        {
            transform.position = Vector2.Lerp(transform.position, destination, .1f);
            yield return null;
        }
        yield return new WaitForSeconds(timeToExplode);
        explosion.SetActive(true);
        GetComponent<AudioSource>().Play();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i=0; i<enemies.Length; i++)
        {
            if((enemies[i].transform.position - transform.position).sqrMagnitude < explosionRadius)
            {
                enemies[i].GetComponent<EnemyController>().Damaged(damage);
            }
        }
        StartCoroutine(CinemachineShake.Instance.ShakeCam(gunShakeAmpl, gunShakeDur));
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
