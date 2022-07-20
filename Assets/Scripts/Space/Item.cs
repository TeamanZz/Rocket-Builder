using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   [SerializeField] private GameObject pickUpEffect;
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         Debug.Log("Item picked up");
         var effect = Instantiate(pickUpEffect, transform.position, Quaternion.identity);
         Destroy(effect,2f);
         Destroy(gameObject);
      }
   }
}
