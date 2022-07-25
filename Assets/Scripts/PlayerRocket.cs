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
    [SerializeField] private float lowFuelTargetPercent;
    private float lowFuelIndicationValue;
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
    [SerializeField] private BuildingGrid buildingGrid;
    public bool isPlayerOnPlanet;

    [Header("WinLoseCondition")]
    private Vector3 startRocketPos;
    [SerializeField] private GameObject loseScreen;
    public Transform rocketContainer;
    public GameObject lowFuelIndication;
    private bool lowFuelEnabled;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startRocketPos = transform.position;
    }

    public void SetRocketVariables()
    {
        currentFuel = startFuel;
        currentShield = startShield;
        lowFuelIndicationValue = startFuel * lowFuelTargetPercent / 100;
        playerFuelBar.fillAmount = 1;
        playerShieldBar.fillAmount = 1;
        oneHitFuelBarFill = playerFuelBar.fillAmount / startFuel;
        oneHitShieldBarFill = playerShieldBar.fillAmount / startShield;

        shieldBarText.text = $"{currentShield} / {startShield}";
    }

    private void FixedUpdate()
    {
        playerFuelBar.fillAmount = oneHitFuelBarFill * currentFuel;
        playerShieldBar.fillAmount = oneHitShieldBarFill * currentShield;

        if (!isPlayerOnPlanet)
            currentFuel -= Time.fixedDeltaTime * fuelDecreaseMultiplier;
        else
            return;

        if (!lowFuelEnabled && currentFuel <= lowFuelIndicationValue)
        {
            lowFuelEnabled = true;
            lowFuelIndication.SetActive(true);
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
        LevelProgress.Instance.CancelFillTween();
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
        // var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        // foreach (var gun in gunsList)
        // {
        //     gun.GetComponent<Gun>().AllowShoot();
        // }
        rocketContainer.localPosition = Vector3.zero;
        rocketContainer.localEulerAngles = new Vector3(0, 0, 0);
        playerFuelBar.gameObject.SetActive(true);
        shieldBarText.gameObject.SetActive(true);
        playerShieldBar.gameObject.SetActive(true);
        transform.localPosition = new Vector3(4, 5, 0);
        loseScreen.SetActive(false);
        DisableLowFuelIndicator();
        this.enabled = false;
    }

    public void DisableLowFuelIndicator()
    {
        lowFuelIndication.SetActive(false);
        lowFuelEnabled = false;
    }
}