using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckWithEnemy : MonoBehaviour
{
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private float explodeDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<EnemyBase>().DescreaseHealth(explodeDamage);
            Destroy(gameObject);
        }
    }

}