using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunDividerBullet : MonoBehaviour
{
    [SerializeField] private float lifeSpan;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private int spread;
    public Vector2 direction;

    private void Start()
    {
        StartCoroutine(WaitforDeath());
        transform.Rotate(0, 0, Random.Range(spread, -spread));
        GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
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
