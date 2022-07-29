
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RocketLauncher : MonoBehaviour, IShootable
{
    public static RocketLauncher Instance; 
        
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
    
    public Transform target;
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float aimDistance;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        if (EnemyManager.Instance.enemiesContainer.Count != 0)
        {
            for (int i = 0; i < EnemyManager.Instance.enemiesContainer.Count; i++)
            {
                var supposeTarget = EnemyManager.Instance.enemiesContainer[i];
                if (aimDistance >= supposeTarget.transform.position.y - transform.position.y)
                {
                    target = supposeTarget.transform;
                }
                else
                {
                    DismissTarget();
                }
            }
        }

        if (target != null)
        {
            if (aimDistance >= (target.position.y - transform.position.y))
            {
                LookAtTarget();
            }
        }
        else if(target == null || target.position.y < transform.position.y)
        {
            DismissTarget();
        }
    }
    
    private void LookAtTarget()
    {
        Vector3 difference = target.position - objectToRotate.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        objectToRotate.transform.rotation = Quaternion.Euler(0.0f, 180f, 90 - rotationZ);
    }

    private void DismissTarget()
    {
        target = null;
        objectToRotate.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

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
