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
    public GameObject deathParticles;

    [SerializeField] private int enemyReward;
    [SerializeField] private float currentHealth;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBg;
    [SerializeField] private float oneHitHealthBarFill;

    [Header("PlayerSettingAndBar")]
    [SerializeField] public float startFuel;
    [SerializeField] public float startShield;
    [SerializeField] private float currentFuel;
    [SerializeField] private float currentShield;
    [SerializeField] private float fuelDecreaseMultiplier;
    [Space(20)]
    [SerializeField] private Image playerFuelBar;
    [SerializeField] private Image playerShieldBar;
    [SerializeField] private Text shieldBarText;
    [Space(20)]
    [SerializeField] private float oneHitFuelBarFill;
    [SerializeField] private float oneHitShieldBarFill;
    [Space(20)]
    public GameObject shieldParticle;
    [SerializeField] private ParticleSystem destroyShieldParticle;
    [SerializeField] private LevelProgress levelProgress;
    [SerializeField] private bool isPlayer; public bool isPlayerOnPlanet;
    [SerializeField] private Money playerMoney;
    [Space(30)]
    [Header("WinLoseCondition")]
    [SerializeField] private GameObject loseScreenPrefab;

    private void Awake()
    {
        playerMoney = FindObjectOfType<Money>();
        levelProgress = FindObjectOfType<LevelProgress>();
        currentHealth = startHealth;
        currentFuel = startFuel;
        if (!isPlayer)
        {
            oneHitHealthBarFill = healthBar.fillAmount / startHealth;
        }
    }

    private void Start()
    {
        if (isPlayer)
        {
            oneHitFuelBarFill = playerFuelBar.fillAmount / startFuel;
            oneHitShieldBarFill = playerShieldBar.fillAmount / startShield;
            currentShield = startShield;
            shieldBarText.text = $"{currentShield} / {startShield}";
        }
        else
        {
            healthBarBg.rectTransform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    private void FixedUpdate()
    {
        if (isPlayer)
        {
            playerFuelBar.fillAmount = oneHitFuelBarFill * currentFuel;
            playerShieldBar.fillAmount = oneHitShieldBarFill * currentShield;

            if (isPlayerOnPlanet)
            {
                return;
            }
            else
            {
                currentFuel -= Time.fixedDeltaTime * fuelDecreaseMultiplier;   
            }

            if (currentFuel <= 0)
            {
                Dead();
            }
            else if(currentShield <= 0)
            {
                currentShield = 0;
            }
        }
    }
    
    public void DecreaseFuel()
    {
        DOTween.To(x => playerFuelBar.fillAmount = x, playerFuelBar.fillAmount,
            0, (startFuel / 20)).SetEase(Ease.Linear);
        DOTween.To(x => currentFuel = x, Mathf.RoundToInt(currentFuel), 0, (startFuel / 20)).SetEase(Ease.Linear);
    }
    

    public void DescreaseHealth(float value)
    {
        if (isPlayer)
        {
            if (currentShield <= 0 || currentShield - value <= 0)
            {
                currentShield = 0;
                destroyShieldParticle.gameObject.SetActive(true);
                shieldParticle.SetActive(false);
                if (currentFuel <= 0)
                {
                    //Dead();
                }
                else
                {
                    currentFuel -= value;
                    RemoveFuelPoints();
                    playerFuelBar.fillAmount = oneHitFuelBarFill * currentFuel;  
                }
            }
            else
            {
                if (currentShield <= 0 || currentShield - value <= 0)
                {
                    destroyShieldParticle.gameObject.SetActive(true);
                    shieldParticle.SetActive(false);
                }
                else
                {
                    currentShield -= value;
                    RemoveShieldPoints();
                    playerShieldBar.fillAmount = oneHitShieldBarFill * currentShield;  
                }
            }
            
            shieldBarText.text = $"{currentShield} / {startShield}";
        }
        else
        {
            if (currentHealth <= 0)
            {
                Instantiate(deathParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
                playerMoney.AddCoins(enemyReward);
            }
            else
            {
                currentHealth -= value;
                RemoveHealth();
                healthBar.fillAmount = oneHitHealthBarFill * currentHealth;
            }
        }

        float randValue = Random.Range(-0.1f, 0.1f);
        transform.DOPunchScale(new Vector3(randValue, randValue, randValue), 0.1f).SetEase(Ease.InBack);

        SFX.Instance.PlayHitSound(gameObject);

        

        // healthBar.fillAmount -= ((float)value / (float)maxHealth);
    }

    public void RemoveHealth()
    {
        DOTween.To(x => healthBar.fillAmount = x, (currentHealth * oneHitHealthBarFill),
            (currentHealth * oneHitHealthBarFill) - oneHitHealthBarFill, 0.1f);
    }

    public void RemoveShieldPoints()
    {
        DOTween.To(x => playerShieldBar.fillAmount = x, (currentShield * oneHitShieldBarFill),
            (currentShield * oneHitShieldBarFill) - oneHitShieldBarFill, 0.1f);
    }

    public void RemoveFuelPoints()
    {
        DOTween.To(x => playerFuelBar.fillAmount = x, (currentFuel * oneHitFuelBarFill),
            (currentFuel * oneHitFuelBarFill) - oneHitFuelBarFill, 0.1f);
    }

    public void Dead()
    {
        levelProgress.CancelFillTween();
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        playerFuelBar.gameObject.SetActive(false);
        shieldBarText.gameObject.SetActive(false);
        playerShieldBar.gameObject.SetActive(false);
        Instantiate(loseScreenPrefab);
        Destroy(gameObject);
    }
}