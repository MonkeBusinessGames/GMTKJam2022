using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Variables")]
    [SerializeField] private float fireRate;
    [SerializeField] private float fireSpread;

    [Header("Assignments")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float bulletTimer;
    private Vector2 mousePosition;

    void Update()
    {
        if (Input.GetMouseButton(0))
            Shoot();
        else if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    // Update is called once per frame
    private void Shoot()
    {
        firePoint.localRotation = Quaternion.Euler(0, 0, firePoint.localRotation.z + Random.Range(-fireSpread, fireSpread)); 
        if (bulletTimer <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletTimer = fireRate;
        }
        bulletTimer -= Time.deltaTime;
        firePoint.localRotation = Quaternion.identity;
    }
}
