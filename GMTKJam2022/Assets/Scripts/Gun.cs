using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{



    [Header("Gun Variables")]
    [SerializeField] public float fireRate;
    [SerializeField] private float fireSpread;
    [SerializeField] private float gunRecoil;
    public string modeName;

    [Header("Assignments")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform parent;
    [Header("Special Weapons")]
    [SerializeField] private bool grenadeLauncher=false;
    public float bulletTimer;


    

    public void Shoot()
    {
        Vector3 moveDirection = parent.position - firePoint.position;
        parent.GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * -gunRecoil);
        firePoint.localRotation = Quaternion.Euler(0, 0, firePoint.localRotation.z + Random.Range(-fireSpread, fireSpread)); 
        if (bulletTimer <= 0)
        {
            if (grenadeLauncher)
            {
                Instantiate(bulletPrefab, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
        }
        firePoint.localRotation = Quaternion.identity;
    }

    
}
