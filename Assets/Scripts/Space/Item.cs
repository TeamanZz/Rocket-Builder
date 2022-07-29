using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool isCollected;
    [SerializeField] private GameObject pickUpEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            isCollected = true;
            NewPartsHandler.Instance.UnlockNewDuplicate();
            Instantiate(pickUpEffect, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            Destroy(gameObject);
        }
    }
}