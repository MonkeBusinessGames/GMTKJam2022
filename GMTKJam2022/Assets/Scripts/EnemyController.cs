using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyState state;
    private static BoxCollider2D player;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;

    [Header("Ranges")]
    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private CircleCollider2D retreatRange;

    private float chaseTimer = .5f;
    private float retreatTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<BoxCollider2D>();
        state = EnemyState.Chasing;
    }

    // Update is called once per frame
    void Update()
    {
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
        switch (state)
        {
            //Move toward player
            case EnemyState.Chasing:
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
                break;

            //Move away from player
            case EnemyState.Retreating:
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.fixedDeltaTime);
                break;

        }
    }
}

public enum EnemyState{
    Chasing,
    Shooting,
    Retreating
};
