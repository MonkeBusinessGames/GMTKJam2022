using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Range(1f, 10.0f)]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float health = 10f;
    [SerializeField] private Gun[] guns;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Camera cam;
    [SerializeField] private GameController gameController;
    private Vector2 mousePosition;
    [SerializeField] private int gunIndex;
    [SerializeField] private int secondaryGunIndex;
    public float bulletTimer;
    [SerializeField] private TextMeshProUGUI moneyText;

    public int money;

    private void Update()
    {
        moneyText.text = "Money: " + money;
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        bulletTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && bulletTimer < 0)
        {
            guns[gunIndex].Shoot();
            bulletTimer = guns[gunIndex].fireRate;
        }
        else if (Input.GetMouseButtonDown(0) && bulletTimer < 0)
        {
            guns[gunIndex].Shoot();
            bulletTimer = guns[gunIndex].fireRate;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            int oldindex = gunIndex;
            while(gunIndex == oldindex)
                gunIndex = Random.Range(0, guns.Length);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            int oldindex = gunIndex;
            gunIndex = secondaryGunIndex;
            secondaryGunIndex = oldindex;
        }
    }

    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(getInput().x * speed, getInput().y * speed);
        Vector2 lookDir = mousePosition - playerRb.position;
        playerRb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    }
    private Vector2 getInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public void Damaged(int damage)
    {
        health -= damage;
        if (health <= 0)
            gameController.GameOver();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            money++;
        }
    }
}