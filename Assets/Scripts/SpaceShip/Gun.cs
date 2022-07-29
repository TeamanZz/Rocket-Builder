using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour, IShootable
{
    public float timeBetweenShots = 0.5f;
    public float damage = 1;
    public float projectileSpeed;
    public GameObject bulletPrefab;
    public Transform muzzle;
    public Transform targetVector;
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
            var newProjectile = Instantiate(bulletPrefab, muzzle.position, Quaternion.Euler(0, 0,transform.parent.rotation.eulerAngles.z), CommonContainer.Instance.transform);
            newProjectile.GetComponent<ProjectileBase>().damage = damage;
            var forceVector = targetVector.position - muzzle.position;
            newProjectile.GetComponent<Rigidbody>().AddForce(forceVector.normalized * projectileSpeed, ForceMode.Impulse);
            OnShoot.Invoke();
            yield return ShootRepeatedely();
        }
    }
}