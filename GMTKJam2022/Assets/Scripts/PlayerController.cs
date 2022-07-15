using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1f, 10.0f)]
    [SerializeField] private float speed = 5f;

    private Rigidbody2D playerRb;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(getInput().x * speed, getInput().y * speed);
    }
    private Vector2 getInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
}