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
    private Money playerMoney;
    private bool isDead;
    private void Awake()
    {
        playerMoney = FindObjectOfType<Money>();
        currentHealth = startHealth;
    }

    private void Start()
    {
        healthBarBg.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void DescreaseHealth(float value)
    {
        if (isDead)
            return;

        currentHealth -= value;
        if (currentHealth <= 0)
        {
            isDead = true;
            Instantiate(deathParticles, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            Instantiate(lootGameobject, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            playerMoney.AddCurrency(enemyReward);
            Destroy(gameObject);
        }
        else
        {
            var newFillAmount = currentHealth / startHealth;
            SetNewFillAmount(newFillAmount);
        }

        float randValue = Random.Range(-0.1f, 0.1f);
        transform.DOPunchScale(new Vector3(randValue, randValue, randValue), 0.1f).SetEase(Ease.InBack);

        SFX.Instance.PlayHitSound(gameObject);
    }

    public void SetNewFillAmount(float newValue)
    {
        DOTween.To(x => healthBar.fillAmount = x, healthBar.fillAmount,
            newValue, 0.1f);
    }
}
