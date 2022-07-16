using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Variables")]
    [SerializeField] public float fireRate;
    [SerializeField] private float fireSpread;

    [Header("Assignments")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    public float bulletTimer;


    public void Shoot()
    {
        firePoint.localRotation = Quaternion.Euler(0, 0, firePoint.localRotation.z + Random.Range(-fireSpread, fireSpread)); 
        if (bulletTimer <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        firePoint.localRotation = Quaternion.identity;
    }
}
