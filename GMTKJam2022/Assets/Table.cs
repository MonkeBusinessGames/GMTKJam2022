using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public string bulletTag;
    public string playerTag;
    public string enemyString;
    public GameObject explosion;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(bulletTag) || other.CompareTag(playerTag) || other.CompareTag(enemyString)){
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
