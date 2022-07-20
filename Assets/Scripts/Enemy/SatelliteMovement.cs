using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteMovement : MonoBehaviour
{
    [SerializeField] private float satelliteSpeed;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private float explodeDamage;

    private void Start()
    {
        //Destroy(gameObject, 6f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(deathParticle,transform.position,Quaternion.identity);
            other.gameObject.GetComponent<EnemyBase>().DescreaseHealth(explodeDamage);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.down * Time.fixedDeltaTime * satelliteSpeed);
    }
}
