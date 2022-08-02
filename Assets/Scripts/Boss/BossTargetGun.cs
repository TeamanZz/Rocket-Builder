using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossTargetGun : MonoBehaviour
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
    [SerializeField] private ParticleSystem chargeBeforeShotParticle;
    private Transform target;
    [SerializeField] private Transform objectToRotate;

    public void AllowShoot()
    {
        gunAnimator = GetComponent<Animator>();
        canShoot = true;
        StartCoroutine(ShootRepeatedely());
    }

    private void Awake()
    {
        target = PlayerRocket.Instance.transform;
    }

    private void Update()
    {
        Vector3 difference = target.position - objectToRotate.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        objectToRotate.transform.rotation = Quaternion.Euler(180, 0, 90 - rotationZ);
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
            chargeBeforeShotParticle.Play();
            yield return new WaitForSeconds(1f);
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

    public IEnumerator OnceBossShot()
    {
        chargeBeforeShotParticle.Play();
        yield return new WaitForSeconds(1f);
        gunAnimator.Play("Shoot",0,0);
        shootParticle.Play();
        var newProjectile = Instantiate(bulletPrefab, muzzle.position, Quaternion.Euler(0, 0,transform.parent.rotation.eulerAngles.z), CommonContainer.Instance.transform);
        newProjectile.GetComponent<ProjectileBase>().damage = damage;
        var forceVector = targetVector.position - muzzle.position;
        newProjectile.GetComponent<Rigidbody>().AddForce(forceVector.normalized * projectileSpeed, ForceMode.Impulse);
        OnShoot.Invoke();
    }
}
