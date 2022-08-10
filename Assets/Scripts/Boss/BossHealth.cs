using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossHealth : MonoBehaviour
{
    public float startHealth;
    [SerializeField] private int enemyReward;
    public GameObject deathParticles;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBg;
    public float currentHealth { get; private set; } = 15;
    private float oneHitHealthBarFill;
    private Money playerMoney;
    [SerializeField] private StartBossFight startBossFight;
    [SerializeField] private GameObject snowParticles;

    private void Awake()
    {
        playerMoney = FindObjectOfType<Money>();
    }

    public void SetHPValue()
    {
        currentHealth = startHealth;
        oneHitHealthBarFill = healthBar.fillAmount / startHealth;
    }

    private void Start()
    {
        healthBarBg.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void DescreaseHealth(float value)
    {
        if (currentHealth <= 0 || currentHealth - value <= 0)
        {
            startBossFight.ReturnSpaceShipValuesOnBossDeath();
            Instantiate(deathParticles, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            gameObject.SetActive(false);
            Instantiate(snowParticles, transform.position, Quaternion.identity);
            playerMoney.AddCurrency(enemyReward);
        }
        else
        {
            currentHealth -= value;
            var newFillAmount = currentHealth / startHealth;
            RemoveHealth(newFillAmount);
        }

        float randValue = Random.Range(-0.01f, 0.01f);
        transform.DOPunchScale(Vector3.one * randValue, 0.05f).SetEase(Ease.InBack);

    }

    public void RemoveHealth(float newValue)
    {
        DOTween.To(x => healthBar.fillAmount = x, healthBar.fillAmount,
            newValue, 0.1f);
    }
}
