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
    [SerializeField] public float startFuel;
    [SerializeField] public float startShield;
    [SerializeField] public float currentFuel;
    [SerializeField] public float currentShield;
    [SerializeField] private float fuelDecreaseMultiplier;
    [Space(20)]
    [SerializeField] private Image playerFuelBar;
    [SerializeField] private Image playerShieldBar;
    [SerializeField] private TextMeshProUGUI shieldBarText;
    [Space(20)]
    [SerializeField] private float oneHitFuelBarFill;
    [SerializeField] private float oneHitShieldBarFill;
    [Space(20)]
    public GameObject shieldParticle;
    [SerializeField] private ParticleSystem destroyShieldParticlePrefab;
    [SerializeField] private LevelProgress levelProgress;
    [SerializeField] private BuildingGrid buildingGrid;
    public bool isPlayerOnPlanet;

    [Header("WinLoseCondition")]
    [SerializeField] private Vector3 startRocketPos;
    [SerializeField] private GameObject loseScreen;
    public Transform rocketContainer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startRocketPos = transform.position;
        currentFuel = startFuel;
        currentShield = startShield;

        oneHitFuelBarFill = playerFuelBar.fillAmount / startFuel;
        oneHitShieldBarFill = playerShieldBar.fillAmount / startShield;

        shieldBarText.text = $"{currentShield} / {startShield}";
    }

    private void FixedUpdate()
    {
        playerFuelBar.fillAmount = oneHitFuelBarFill * currentFuel;
        playerShieldBar.fillAmount = oneHitShieldBarFill * currentShield;

        if (!isPlayerOnPlanet)
        {
            currentFuel -= Time.fixedDeltaTime * fuelDecreaseMultiplier;
        }
        else
        {
            return;
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

    [ContextMenu("Test Dead")]
    public void Dead()
    {
        levelProgress.CancelFillTween();
        playerFuelBar.gameObject.SetActive(false);
        shieldBarText.gameObject.SetActive(false);
        playerShieldBar.gameObject.SetActive(false);
        SpaceShipMovement.Instance.SetZeroVariables();
        loseScreen.SetActive(true);
        Instantiate(destroyShieldParticlePrefab);
        gameObject.SetActive(false);
    }

    [ContextMenu("Restart Rocket")]
    public void RestartRocket()
    {
        currentFuel = startFuel;
        currentShield = startShield;
        transform.position = startRocketPos;
        var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
        {
            gun.GetComponent<Gun>().AllowShoot();
        }
        rocketContainer.localPosition = Vector3.zero;
        levelProgress.RestartFillTween();
        playerFuelBar.gameObject.SetActive(true);
        shieldBarText.gameObject.SetActive(true);
        playerShieldBar.gameObject.SetActive(true);
        transform.localPosition = new Vector3(4, 5, 0);
        loseScreen.SetActive(false);
        this.enabled = false;
    }
}