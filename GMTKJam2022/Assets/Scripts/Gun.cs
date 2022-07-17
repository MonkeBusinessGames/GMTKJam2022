using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Variables")]
    [SerializeField] public float fireRate;
    [SerializeField] private float fireSpread;
    [SerializeField] private float gunRecoil;
    [SerializeField] private float gunShakeAmpl;
    [SerializeField] private float gunShakeDur;
    public string modeName;

    [Header("Assignments")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform recoilPoint;
    [SerializeField] private Transform firePoint;
    [SerializeField] private SpriteRenderer gunRend;
    private Quaternion bulletRotation;
    [Header("Special Weapons")]
    [SerializeField] private bool grenadeLauncher = false;
    [SerializeField] private bool shotgun = false;
    public Transform gun;
    public float bulletTimer;

    public void Shoot()
    {
        Vector3 moveDirection = firePoint.position - recoilPoint.position;
        GetComponentInParent<Rigidbody2D>().AddForce(moveDirection.normalized * gunRecoil);
        gun.localRotation = Quaternion.Euler(0, 0, firePoint.localRotation.z + Random.Range(-fireSpread, fireSpread));
        if (gunRend.flipX)
            bulletRotation = Quaternion.Euler(new Vector3(0, 0, firePoint.eulerAngles.z + 180));
        else
            bulletRotation = Quaternion.Euler(new Vector3(0, 0, firePoint.eulerAngles.z + 180));

        if (bulletTimer <= 0)
        {
            StartCoroutine(CinemachineShake.Instance.ShakeCam(gunShakeAmpl, gunShakeDur));
            if (grenadeLauncher)
            {
                Grenade grenade = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Grenade>();
                grenade.destination = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            }
            else if(shotgun)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<ShotgunPlayerBullet>().Shoot(moveDirection);
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<PlayerBullet>().Shoot(moveDirection);
            }
        }
        firePoint.localRotation = Quaternion.identity;
    }

    
}
