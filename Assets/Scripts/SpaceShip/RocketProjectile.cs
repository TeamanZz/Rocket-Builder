using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RocketProjectile : ProjectileBase
{
    public bool wasCollided;
    public GameObject hitParticles;

    private Transform target;
    public Rigidbody rb;

    private RocketLauncher rocketLauncher;

    private void Start()
    {
        StartCoroutine(SpawnParticleAfterDestroy());
    }

    private void Update()
    {
        // if (target != null)
        // {
        //     transform.LookAt(target.transform.position, Vector3.right);
        //     rb.AddRelativeForce(Vector3.forward, ForceMode.Impulse);
        // }
        // if (RocketLauncher.Instance.target != null)
        // {
        //     transform.position = Vector3.Lerp(transform.position, RocketLauncher.Instance.target.position, Time.deltaTime / 2);
        // }

    }

    public void InitializeRocket(float newDamage, Transform newTarget, RocketLauncher parentRocketLauncher)
    {
        rocketLauncher = parentRocketLauncher;
        damage = newDamage;
        target = newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy;
        if (!wasCollided && other.gameObject.TryGetComponent<EnemyBase>(out enemy))
        {
            Vector3 particlesPosition = new Vector3(enemy.transform.position.x + Random.Range(-0.5f, 0.5f), enemy.transform.position.y + Random.Range(-1f, 0f), enemy.transform.position.z - 1);
            var hitParticle = Instantiate(hitParticles, particlesPosition, Quaternion.identity, CommonContainer.Instance.transform);
            wasCollided = true;
            enemy.DescreaseHealth(damage);
            Destroy(gameObject);
        }

        LootContainer lootContainer;
        if (!wasCollided && other.gameObject.TryGetComponent<LootContainer>(out lootContainer))
        {
            Vector3 particlesPosition = new Vector3(lootContainer.transform.position.x + Random.Range(-0.5f, 0.5f), lootContainer.transform.position.y + Random.Range(-1f, 0f), lootContainer.transform.position.z - 1);
            var hitParticle = Instantiate(hitParticles, particlesPosition, Quaternion.identity, CommonContainer.Instance.transform);
            wasCollided = true;
            lootContainer.DescreaseHealth(damage);
            Destroy(gameObject);
        }

        EnemyProjectile enemyProjectile;
        if (!wasCollided && other.gameObject.TryGetComponent<EnemyProjectile>(out enemyProjectile))
        {
            Vector3 particlesPosition = new Vector3(enemyProjectile.transform.position.x + Random.Range(-0.5f, 0.5f), enemyProjectile.transform.position.y + Random.Range(-1f, 0f), enemyProjectile.transform.position.z - 1);
            var hitParticle = Instantiate(hitParticles, particlesPosition, Quaternion.identity, CommonContainer.Instance.transform);
            wasCollided = true;
            enemyProjectile.DescreaseHealth(damage);
            Destroy(gameObject);
        }
    }
    private IEnumerator SpawnParticleAfterDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        var hitParticle = Instantiate(hitParticles, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
        Destroy(hitParticle, 2f);
        Destroy(gameObject);
    }
}