using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float lifeSpan;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
        StartCoroutine(WaitforDeath());
    }

    private IEnumerator WaitforDeath()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().Damaged(damage);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
            Destroy(gameObject);
    }
}
