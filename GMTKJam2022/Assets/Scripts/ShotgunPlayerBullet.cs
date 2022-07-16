using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPlayerBullet : MonoBehaviour
{
    [SerializeField] private float lifeSpan;
    [SerializeField] private float speed;
    [SerializeField] private float fireSpread;
    [SerializeField] private int bulletsAmount;
    [SerializeField] private GameObject bullet;


    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
        StartCoroutine(WaitforDeath());
        for(int i=0; i<bulletsAmount; i++)
        {
            GameObject bullets = Instantiate(bullet, transform.position, transform.rotation);
        }
    }

    private IEnumerator WaitforDeath()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
}
