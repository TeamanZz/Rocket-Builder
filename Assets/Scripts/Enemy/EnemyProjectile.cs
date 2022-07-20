using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
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
        /*SpaceShipHealth playerShip;
        if(!wasCollided && other.gameObject.TryGetComponent<SpaceShipHealth>(out playerShip))
        {
            Vector3 particlesPosition = new Vector3(playerShip.transform.position.x + Random.Range(-0.5f, 0.5f), playerShip.transform.position.y + Random.Range(-1f, 0f), playerShip.transform.position.z - 1);
            Instantiate(hitParticles, particlesPosition, Quaternion.identity);
            wasCollided = true;
            playerShip.TakeDamage(damage);
            Destroy(gameObject);
        }*/

        if (other.gameObject.GetComponent<SpaceShipHealth>())
        {
            SpaceShipHealth playerShip = other.gameObject.GetComponent<SpaceShipHealth>();
            Vector3 particlesPosition = new Vector3(playerShip.transform.position.x + Random.Range(-0.5f, 0.5f), playerShip.transform.position.y + Random.Range(-1f, 0f), playerShip.transform.position.z - 1);
            Instantiate(hitParticles, particlesPosition, Quaternion.identity);
            playerShip.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
