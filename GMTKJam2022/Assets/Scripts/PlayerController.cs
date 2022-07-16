using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1f, 10.0f)]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float health = 10f;

    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Camera cam;
    [SerializeField] private GameController gameController;
    private Vector2 mousePosition;

    [Header("Gun Fields")]
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float bulletTimer;



    private void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
            Shoot();
        else if (Input.GetMouseButtonUp(0))
            bulletTimer = 0;
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

    private void Shoot()
    {
        if(bulletTimer <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletTimer = fireRate;
        }
        bulletTimer -= Time.deltaTime;
    }

    public void Damaged(int damage)
    {
        health -= damage;
        if (health <= 0)
            gameController.GameOver();
    }
}