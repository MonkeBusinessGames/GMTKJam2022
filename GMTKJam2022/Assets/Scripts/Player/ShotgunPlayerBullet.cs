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
    private Vector2 bulletDirection;


    private void Start()
    {
        StartCoroutine(WaitforDeath());
        for(int i=0; i<bulletsAmount; i++)
        {
            GameObject bullets = Instantiate(bullet, transform.position, transform.rotation);
            bullets.GetComponent<ShotgunDividerBullet>().direction = bulletDirection;
        }
    }

    private IEnumerator WaitforDeath()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    public void Shoot(Vector2 direction)
    {
        bulletDirection = direction;
        GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
