using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFX : MonoBehaviour
{
    float destroyTime=5f;
    void Start()
    {
        Destroy(gameObject, destroyTime);  
    }
}
