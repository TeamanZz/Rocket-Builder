using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipHealth : MonoBehaviour
{
     public float health;

     public void TakeDamage(float givenDamage)
     {
          health -= givenDamage;
          Debug.Log($"Current health = {health}");
     }
}
