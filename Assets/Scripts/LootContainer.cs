using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LootContainer : MonoBehaviour
{
    public float startHealth;
    private float currentHealth;
    [SerializeField] private int enemyReward;
    public GameObject deathParticles;
    public GameObject lootGameobject;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBg;
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
    }

    public void DescreaseHealth(float value)
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            Instantiate(lootGameobject, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            playerMoney.AddCurrency(enemyReward);
            Destroy(gameObject);
        }
        else
        {
            currentHealth -= value;
            RemoveHealth();
        }

        float randValue = Random.Range(-0.1f, 0.1f);
        transform.DOPunchScale(new Vector3(randValue, randValue, randValue), 0.1f).SetEase(Ease.InBack);

        SFX.Instance.PlayHitSound(gameObject);
    }

    public void RemoveHealth()
    {
        DOTween.To(x => healthBar.fillAmount = x, (currentHealth * oneHitHealthBarFill),
            (currentHealth * oneHitHealthBarFill) - oneHitHealthBarFill, 0.1f);
    }
}
