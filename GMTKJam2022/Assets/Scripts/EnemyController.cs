using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private EnemyState state;
    private static BoxCollider2D player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Canvas enemyCanvas;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;

    [Header("Ranges")]
    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private CircleCollider2D retreatRange;

    [SerializeField] private Gun gun;
    private float bulletTimer;
    /*[Header("Gun Fields")]
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float bulletTimer*/


    private float chaseTimer = .5f;
    private float retreatTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = healthBar.value = health;
        
        GameController.enemyCount++;
        player = FindObjectOfType<PlayerController>().GetComponent<BoxCollider2D>();
        state = EnemyState.Chasing;
    }

    // Update is called once per frame
    void Update()
    {
        bulletTimer -= Time.deltaTime;
        switch (state)
        {
            case EnemyState.Chasing:
                //If the player is in range, start shooting
                if (attackRange.IsTouching(player))
                {
                    chaseTimer -= Time.deltaTime;
                    if(chaseTimer <= 0)
                        state = EnemyState.Shooting;
                    break;
                }
                chaseTimer = .5f;
                //If the player is out of range, keep chasing
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                break;
            case EnemyState.Shooting:
                //If the player is out of range, start chasing
                if (!attackRange.IsTouching(player))
                {
                    state = EnemyState.Chasing;
                    break;
                }
                //If the player is too close, start retreating
                if (retreatRange.IsTouching(player))
                {
                    state = EnemyState.Retreating;
                    retreatTimer = 1;
                    break;
                }
                if (bulletTimer < 0)
                {
                    gun.Shoot();
                    bulletTimer = gun.fireRate;
                }

                break;
            case EnemyState.Retreating:
                //If the player is too close, keep retreating
                if (retreatRange.IsTouching(player))
                {
                    retreatTimer = 1;
                    break;
                }
                retreatTimer -= Time.deltaTime;
                if (retreatTimer <= 0)
                {
                    //If the player is in range, start shooting
                    if (attackRange.IsTouching(player))
                    {
                        state = EnemyState.Shooting;
                        break;
                    }

                    //If the player is out of range, start chasing
                    state = EnemyState.Chasing;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = (Vector2)player.transform.position - rb.position;

        switch (state)
        {
            //Move toward player
            case EnemyState.Chasing:
                lookDir = (Vector2) player.transform.position - rb.position;
                rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                enemyCanvas.transform.rotation = Quaternion.identity;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
                break;

            case EnemyState.Shooting:
                lookDir = (Vector2)player.transform.position - rb.position;
                rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                enemyCanvas.transform.rotation = Quaternion.identity;
                break;

            //Move away from player
            case EnemyState.Retreating:
                lookDir = (Vector2)player.transform.position - rb.position;
                rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
                enemyCanvas.transform.rotation = Quaternion.identity;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.fixedDeltaTime);
                break;

        }
    }

    public void Damaged(int damage)
    {
        health -= damage;
        healthBar.value = health;
        if (health <= 0)
        {
            GameController.enemyCount--;
            Destroy(gameObject);
        }
    }
}

public enum EnemyState{
    Chasing,
    Shooting,
    Retreating
};
