using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float bulletTimer;

    private void Update()
    {
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
            gunIndex = Random.Range(0, guns.Length);
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
}