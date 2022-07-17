using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private EnemyState state;
    private static BoxCollider2D player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sRend;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private GameObject deathSoundPlayer;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Canvas enemyCanvas;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float recoverTime = .5f;

    [Header("Ranges")]
    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private CircleCollider2D retreatRange;

    [SerializeField] private Gun gun;
    private float bulletTimer;
    private bool damaged;

    [SerializeField] private GameObject money;
    [SerializeField] private float moneyAmount;
    [SerializeField] private float moneyDropRadius;

    [Header("Sound")]
    [SerializeField] private AudioClip[] deathSFX;
    /*[Header("Gun Fields")]
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float bulletTimer*/

    private Animator animator;
    private AudioSource audioSource;


    private float chaseTimer = .5f;
    private float retreatTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = healthBar.value = health;
        
        GameController.enemyCount++;
        player = FindObjectOfType<PlayerController>().GetComponent<BoxCollider2D>();
        state = EnemyState.Chasing;
        if(spriteObject != null)
        {
            sRend = spriteObject.GetComponent<SpriteRenderer>();
            animator = spriteObject.GetComponent<Animator>();
        }
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
        if(animator != null)
        {
            SetAnim(state);
        }
    }


    private void FixedUpdate()
    {

        if (damaged)
            return;

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
        if (damaged)
            return;
        damaged = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        health -= damage;
        healthBar.value = health;
        if (health <= 0)
        {
            playDeathSound();
            GameController.enemyCount--;
            for(int i=0; i< moneyAmount; i++)
            {
                Instantiate(money, new Vector3(transform.position.x + Random.Range(-moneyDropRadius, moneyDropRadius), transform.position.y + Random.Range(-moneyDropRadius, moneyDropRadius), 0), Quaternion.identity);
            }
            Destroy(gameObject);
        }
        StartCoroutine(DamageWait());
    }

    IEnumerator DamageWait()
    {
        //sRend.color = new Color(1, 0, 0, .5f);

        print("damaged");
        yield return new WaitForSeconds(recoverTime);

        //sRend.color = Color.red;
        print("recovered");
        damaged = false;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    private void SetAnim(EnemyState state)
    {

        if (state == EnemyState.Shooting && animator.GetBool("isFiring") != true)
        {
            StartCoroutine(PlayShootAnim());
        }
    }

    IEnumerator PlayShootAnim()
    {
        animator.SetBool("isFiring", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("isFiring", false);
    }

    private void playDeathSound()
    {
        if(deathSoundPlayer!= null)
        {
            Vector2 spawnPos = transform.position;
            GameObject obj = Instantiate(deathSoundPlayer, spawnPos, Quaternion.identity);
            obj.GetComponent<DeathSoundPlayer>().PlaySound(deathSFX);
        }
    }
}



public enum EnemyState{
    Chasing,
    Shooting,
    Retreating
};
