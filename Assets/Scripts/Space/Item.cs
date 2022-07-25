using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject pickUpEffect;
    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            Instantiate(pickUpEffect, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            Destroy(gameObject);
        }
    }
}