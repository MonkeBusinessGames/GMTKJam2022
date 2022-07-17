using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1f, 10.0f)]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float health = 10f;
    [SerializeField] private Gun[] guns;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private SpriteRenderer sRend;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite moveSprite;
    [SerializeField] private Sprite hitSprite;
    [SerializeField] private Camera cam;
    [SerializeField] private GameController gameController;
    private Vector2 mousePosition;
    [SerializeField] private int gunIndex;
    [SerializeField] private int lowestRandomTime = 5;
    [SerializeField] private int highestRandomTime = 20;
    [SerializeField] private int secondaryGunIndex;
    [SerializeField] private float recoverTime = .5f;
    [SerializeField] private Image reloadUIII;
    public float bulletTimer;
    private bool damaged;


    public int money;
    private float gunChangeRandom;

    private void Start()
    {
        gunChangeRandom = Random.Range(lowestRandomTime, highestRandomTime);
        //Start with random gun
        int oldindex = gunIndex;
        while (gunIndex == oldindex)
            gunIndex = Random.Range(0, guns.Length);
        gameController.modeUpdate(guns[gunIndex].modeName);
    }

    private void Update()
    {
        
        reloadUIII.fillAmount = 1 - (bulletTimer/guns[gunIndex].fireRate);
        gunChangeRandom -= Time.deltaTime;
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        bulletTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && bulletTimer < 0)
        {
            guns[gunIndex].Shoot();
            GunSoundManager.Instance.PlayGunSFX(gunIndex);
            bulletTimer = guns[gunIndex].fireRate;
        }
        if(!Input.GetMouseButton(0) && GunSoundManager.Instance.machineGunSoundIsPlaying)
        {
            GunSoundManager.Instance.PlayGunTail();
        }
        /*else if (Input.GetMouseButtonDown(0) && bulletTimer < 0)
        {
            guns[gunIndex].Shoot();
            bulletTimer = guns[gunIndex].fireRate;
        }*/
        if (gunChangeRandom<=0)
        {
            gunChangeRandom = Random.Range(lowestRandomTime, highestRandomTime);
            int oldindex = gunIndex;
            while(gunIndex == oldindex)
                gunIndex = Random.Range(0, guns.Length);
            gameController.modeUpdate(guns[gunIndex].modeName);
            StartCoroutine(gameController.ShowSwitchVisual());
        }
        if (Input.GetMouseButtonDown(1))
        {
            int oldindex = gunIndex;
            gunIndex = secondaryGunIndex;
            secondaryGunIndex = oldindex;
            gameController.modeUpdate(guns[gunIndex].modeName, guns[secondaryGunIndex].modeName);
        }
    }

    void FixedUpdate()
    {
        if (damaged)
            return;
        playerRb.velocity = new Vector2(getInput().x * speed, getInput().y * speed);
        Vector2 lookDir = mousePosition - playerRb.position;
        playerRb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        if (playerRb.velocity.magnitude > 0)
            sRend.sprite = moveSprite;
        else
            sRend.sprite = idleSprite;

    }
    private Vector2 getInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public void Damaged(int damage)
    {
        if (damaged)
            return;
        damaged = true;
        //Cinemachine Shake
        StartCoroutine(CinemachineShake.Instance.ShakeCam(5f, .1f));
        sRend.sprite = hitSprite;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        health -= damage;
        gameController.UpdateHealth(damage);
        if (health <= 0)
            gameController.GameOver();

        StartCoroutine(DamageWait());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            money++;
            gameController.UpdateScore(money);
        }
    }

    IEnumerator DamageWait()
    {
        sRend.color = new Color(1, 1, 1, .9f);

        print("damaged");
        yield return new WaitForSeconds(recoverTime);

        sRend.color = Color.white;
        print("recovered");
        damaged = false;
        playerRb.constraints = RigidbodyConstraints2D.None;
    }
}