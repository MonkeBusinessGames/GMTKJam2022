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
    [SerializeField] private SpriteRenderer gunRend;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private GameObject deathSoundPlayer;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Canvas enemyCanvas;

    [SerializeField] private Sprite sideSprite;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite hitSprite;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float recoverTime = .5f;

    [Header("Ranges")]
    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private CircleCollider2D retreatRange;

    [Header("FX")]
    [SerializeField] private GameObject deathFX;

    [SerializeField] private EnemyGun gun;
    private float bulletTimer;
    private bool damaged;

    [SerializeField] private GameObject money;
    [SerializeField] private Transform firePoint;
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
                //rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                enemyCanvas.transform.rotation = Quaternion.identity;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
                break;

            case EnemyState.Shooting:
                lookDir = (Vector2)player.transform.position - rb.position;
                //rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                enemyCanvas.transform.rotation = Quaternion.identity;
                break;

            //Move away from player
            case EnemyState.Retreating:
                lookDir = (Vector2)player.transform.position - rb.position;
                //rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
                enemyCanvas.transform.rotation = Quaternion.identity;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.fixedDeltaTime);
                break;

        }

        float gunAngle = 0;

        //Right
        if (lookDir.x > 0)
        {
            //Up and Right
            if (lookDir.y > lookDir.x)
            {
                sRend.flipX = false;
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                sRend.sprite = upSprite;
                transform.eulerAngles = Vector3.zero;
            }
            //Down and Right
            else if ((lookDir.y * -1) > lookDir.x)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                sRend.sprite = sideSprite;
                transform.eulerAngles = new Vector3(0, 0, -20);
            }
            //Only Right
            else
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                sRend.sprite = sideSprite;
                transform.eulerAngles = Vector3.zero;
            }
        }
        //Left
        else if (lookDir.x < 0)
        {
            //Down and Left
            if (lookDir.y < lookDir.x)
            {
                gunAngle = 180;
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                sRend.sprite = sideSprite;
                transform.eulerAngles = new Vector3(0, 0, 20);

            }
            //Up and Left
            else if ((lookDir.y * -1) < lookDir.x)
            {
                gunAngle = 180;
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                sRend.sprite = upSprite;
                transform.eulerAngles = Vector3.zero;
            }
            //Only Left
            else
            {
                gunAngle = 180;
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                sRend.sprite = sideSprite;
                transform.eulerAngles = Vector3.zero;
            }
        }

        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + gunAngle);
    }

    public void Damaged(int damage)
    {
        if (damaged)
            return;
        damaged = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        health -= damage;
        healthBar.value = health;
        sRend.sprite = hitSprite;
        if (health <= 0)
        {
            playDeathSound();
            GameController.enemyCount--;
            for(int i=0; i< moneyAmount; i++)
            {
                Instantiate(money, new Vector3(transform.position.x + Random.Range(-moneyDropRadius, moneyDropRadius), transform.position.y + Random.Range(-moneyDropRadius, moneyDropRadius), 0), Quaternion.identity);
            }
            Destroy(gameObject);
            Instantiate(deathFX, transform.position, Quaternion.identity);
        }
        StartCoroutine(DamageWait());
    }

    IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(recoverTime);

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
