using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float startHealth;
    public float currentHealth;
    public GameObject deathParticles;

    private void Awake()
    {
        currentHealth = startHealth;
    }

    public void DescreaseHealth(float value)
    {
        Debug.Log("Decrease");
        currentHealth -= value;
        // healthBar.fillAmount -= ((float)value / (float)maxHealth);
        if (currentHealth <= 0)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}