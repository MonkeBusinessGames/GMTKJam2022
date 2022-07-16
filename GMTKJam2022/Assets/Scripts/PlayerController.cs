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

    private void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
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