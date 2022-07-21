using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private GameObject itemOnDestroy;

    private void OnTriggerEnter(Collider other)
    {
        SpaceShipMovement playerComponent;
        if (other.TryGetComponent<SpaceShipMovement>(out playerComponent))
        {
            var effect = Instantiate(itemOnDestroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}