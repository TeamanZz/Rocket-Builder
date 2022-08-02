using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    public float startHealth;
    [SerializeField] private int enemyReward;
    public GameObject deathParticles;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBg;
    private float currentHealth;
    private float oneHitHealthBarFill;
    private Money playerMoney;

    private void Awake()
    {
        playerMoney = FindObjectOfType<Money>();
        currentHealth = startHealth;

        oneHitHealthBarFill = healthBar.fillAmount / startHealth;
    }

    private void Start()
    {
        healthBarBg.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        EnemyManager.Instance.AddEnemyAtEnemiesContainer(this);
    }

    public void DescreaseHealth(float value)
    {
        if (currentHealth <= 0 || currentHealth - value <= 0)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            EnemyManager.Instance.RemoveEnemyFromContainer(this);
            Destroy(gameObject);
            playerMoney.AddCurrency(enemyReward);
        }
        else
        {
            currentHealth -= value;
            var newFillAmount = currentHealth / startHealth;
            RemoveHealth(newFillAmount);
        }

        float randValue = Random.Range(-0.1f, 0.1f);
        transform.DOPunchScale(Vector3.one * randValue, 0.1f).SetEase(Ease.InBack);

        SFX.Instance.PlayHitSound(gameObject);
    }

    public void RemoveHealth(float newValue)
    {
        DOTween.To(x => healthBar.fillAmount = x, healthBar.fillAmount,
            newValue, 0.1f);
    }
    
}