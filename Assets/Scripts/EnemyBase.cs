using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    public float startHealth;
    public GameObject deathParticles;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = startHealth;
    }

    public void DescreaseHealth(float value)
    {
        currentHealth -= value;

        float randValue = Random.Range(-0.1f, 0.1f);
        transform.DOPunchScale(new Vector3(randValue, randValue, randValue), 0.1f).SetEase(Ease.InBack);

        SFX.Instance.PlayHitSound(gameObject);

        if (currentHealth <= 0)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        // healthBar.fillAmount -= ((float)value / (float)maxHealth);
    }
}