using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public float timeBetweenShots = 0.5f;
    public float damage = 1;
    public GameObject bulletPrefab;
    public Transform muzzle;
    public Transform targetVector;
    public Transform ship;
    public UnityEvent OnShoot;
    public bool canShoot = true;

    [SerializeField] private bool isEnemyGun;
    [SerializeField] private float enemyProjectileSpeed;

    private void Start()
    {
        StartCoroutine(ShootRepeatedely());
        ship = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private IEnumerator ShootRepeatedely()
    {
        if (canShoot)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            var newProjectile = Instantiate(bulletPrefab, muzzle.position, Quaternion.Euler(0, 0, -ship.eulerAngles.z));
            newProjectile.GetComponent<Projectile>().damage = damage;
            var forceVector = targetVector.position - muzzle.position;
            if (isEnemyGun)
            {
                newProjectile.GetComponent<Rigidbody>()
                    .AddForce(forceVector.normalized * enemyProjectileSpeed, ForceMode.Impulse);
            }
            else
            {
                newProjectile.GetComponent<Rigidbody>().AddForce(forceVector.normalized * 20, ForceMode.Impulse);
            }

            OnShoot.Invoke();
            yield return ShootRepeatedely();
        }
    }

    public void AllowShoot()
    {
        canShoot = true;
        StartCoroutine(ShootRepeatedely());
    }
}