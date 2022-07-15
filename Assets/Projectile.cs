using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage = 1;
    public bool wasCollided;
    public GameObject hitParticles;

    private void Start()
    {
        Destroy(gameObject, 0.65f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy;
        if (!wasCollided && other.gameObject.TryGetComponent<Enemy>(out enemy))
        {
            Vector3 particlesPosition = new Vector3(enemy.transform.position.x + Random.Range(-0.5f, 0.5f), enemy.transform.position.y + Random.Range(-1f, 0f), enemy.transform.position.z - 1);
            Instantiate(hitParticles, particlesPosition, Quaternion.identity);
            wasCollided = true;
            enemy.DescreaseHealth(damage);
            Destroy(gameObject);
        }
    }
}