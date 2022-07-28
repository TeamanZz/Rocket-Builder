using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleGun : MonoBehaviour, IShootable
{
    public float timeBetweenShots = 0.5f;
    public float damage = 1;
    public float projectileSpeed;
    public GameObject bulletPrefab;

    public Transform muzzle;
    public Transform secondMuzzle;

    public Transform targetVector;
    public Transform secondTargetVector;

    public UnityEvent OnShoot;
    public bool canShoot = true;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private ParticleSystem shootParticle; 

    public void AllowShoot()
    {
        gunAnimator = GetComponent<Animator>();
        canShoot = true;
        StartCoroutine(ShootRepeatedely());
    }

    public void ForbidShoot()
    {
        canShoot = false;
        StopCoroutine(ShootRepeatedely());
    }

    public IEnumerator ShootRepeatedely()
    {
        if (canShoot)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            gunAnimator.Play("Shoot",0,0);
            shootParticle.Play();
            var projectile = Instantiate(bulletPrefab, muzzle.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), CommonContainer.Instance.transform);
            var secondProjectile = Instantiate(bulletPrefab, secondMuzzle.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), CommonContainer.Instance.transform);

            projectile.GetComponent<ProjectileBase>().damage = damage;
            secondProjectile.GetComponent<ProjectileBase>().damage = damage;

            var forceVector = targetVector.position - muzzle.position;
            var secondForceVector = secondTargetVector.position - secondMuzzle.position;

            projectile.GetComponent<Rigidbody>().AddForce(forceVector.normalized * projectileSpeed, ForceMode.Impulse);
            secondProjectile.GetComponent<Rigidbody>().AddForce(secondForceVector.normalized * projectileSpeed, ForceMode.Impulse);

            OnShoot.Invoke();
            yield return ShootRepeatedely();
        }
    }
}