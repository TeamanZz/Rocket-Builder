using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool isCollected;
    [SerializeField] private GameObject pickUpEffect;

    private void Start()
    {
       // Destroy(gameObject, 10);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LootCollected");
        if (isCollected) return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            isCollected = true;
            var lootTriggers = LevelsHandler.Instance.GetCurrentLootTriggers();
            for (var i = 0; i < lootTriggers.Count; i++)
            {
                lootTriggers[i].currentDuplicatesValue++;
            }
            NewPartsHandler.Instance.UnlockNewDuplicate();
            Instantiate(pickUpEffect, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            Destroy(gameObject);
        }
    }
}