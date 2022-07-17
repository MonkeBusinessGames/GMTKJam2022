using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    public float timer;
    void Start()
    {
        Destroy(gameObject, timer);
    }

    
}
