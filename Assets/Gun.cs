using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float timeBetweenShots = 0.5f;
    public float damage = 1;
    public Transform muzzle;
    public Transform targetVector;
    public Transform ship;

    private void Start()
    {
        StartCoroutine(ShootRepeatedely());
    }

    private IEnumerator ShootRepeatedely()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            var newProjectile = Instantiate(bulletPrefab, muzzle.position, Quaternion.Euler(0, 0, -ship.eulerAngles.z));
            newProjectile.GetComponent<Projectile>().damage = damage;
            var forceVector = targetVector.position - muzzle.position;
            newProjectile.GetComponent<Rigidbody>().AddForce(forceVector.normalized * 20, ForceMode.Impulse);
        }
    }
}