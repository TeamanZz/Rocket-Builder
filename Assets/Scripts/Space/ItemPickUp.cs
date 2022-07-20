using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private GameObject itemOnDestroy;

    private void OnDestroy()
    {
        Instantiate(itemOnDestroy,transform.position,Quaternion.identity);
    }
}
