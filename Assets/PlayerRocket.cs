using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerRocket : MonoBehaviour
{
    public static PlayerRocket Instance;

    [Header("PlayerSettingAndBar")]
    [HideInInspector] public float startFuel;
    [HideInInspector] public float startShield;
    private float currentFuel;
    private float currentShield;
    [SerializeField] private float fuelDecreaseMultiplier;
    [Space(20)]
    [SerializeField] private Image playerFuelBar;
    [SerializeField] private Image playerShieldBar;
    [SerializeField] private TextMeshProUGUI shieldBarText;
    [Space(20)]
    private float oneHitFuelBarFill;
    private float oneHitShieldBarFill;
    [Space(20)]
    public GameObject shieldParticle;
    [SerializeField] private ParticleSystem destroyShieldParticlePrefab;
    private LevelProgress levelProgress;
    public bool isPlayerOnPlanet;
    [Header("WinLoseCondition")]
    [SerializeField] private GameObject loseScreen;
    public Transform rocketContainer;

    private void Awake()
    {
        Instance = this;
        levelProgress = FindObjectOfType<LevelProgress>();
        currentFuel = startFuel;
    }

    private void Start()
    {
        oneHitFuelBarFill = playerFuelBar.fillAmount / startFuel;
        oneHitShieldBarFill = playerShieldBar.fillAmount / startShield;
        currentShield = startShield;
        shieldBarText.text = $"{currentShield} / {startShield}";
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

    public void DescreaseHealth(float value)
    {
        if (currentShield <= 0 || currentShield - value <= 0)
        {
            currentShield = 0;
            Instantiate(destroyShieldParticlePrefab, transform.position, Quaternion.identity);
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
                Instantiate(destroyShieldParticlePrefab, transform.position, Quaternion.identity);
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

        float randValue = Random.Range(-0.1f, 0.1f);
        transform.DOPunchScale(new Vector3(randValue, randValue, randValue), 0.1f).SetEase(Ease.InBack);

        SFX.Instance.PlayHitSound(gameObject);
    }

    public void Dead()
    {
        levelProgress.CancelFillTween();
        playerFuelBar.gameObject.SetActive(false);
        shieldBarText.gameObject.SetActive(false);
        playerShieldBar.gameObject.SetActive(false);
        loseScreen.SetActive(true);
        Destroy(gameObject);
    }

    private void FixedUpdate()
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
        else if (currentShield <= 0)
        {
            currentShield = 0;
        }
    }
}