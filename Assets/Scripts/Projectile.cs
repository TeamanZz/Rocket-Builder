using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage = 1;
    public bool wasCollided;
    public GameObject hitParticles;

    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    private void OnDestroy()
    {
        Instantiate(hitParticles, gameObject.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy;
        if (!wasCollided && other.gameObject.TryGetComponent<EnemyBase>(out enemy))
        {
            Vector3 particlesPosition = new Vector3(enemy.transform.position.x + Random.Range(-0.5f, 0.5f), enemy.transform.position.y + Random.Range(-1f, 0f), enemy.transform.position.z - 1);
            var hitParticle = Instantiate(hitParticles, particlesPosition, Quaternion.identity);
            Destroy(hitParticle,1f);
            wasCollided = true;
            enemy.DescreaseHealth(damage);
            Destroy(gameObject);
        }
    }
}