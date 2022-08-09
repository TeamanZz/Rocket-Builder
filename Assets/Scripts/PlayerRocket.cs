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
    [SerializeField] public float fuelDecreaseMultiplier;
    [SerializeField] private Image playerFuelBar;
    [SerializeField] private Image playerShieldBar;
    [SerializeField] private TextMeshProUGUI shieldBarText;
    [SerializeField] private float oneHitFuelBarFill;
    [SerializeField] private float oneHitShieldBarFill;
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
    public Rigidbody rb;
    public Cinemachine.CinemachineVirtualCamera cineCamera;
    public bool isDead;
    public Gradient fuelGraident;

    public GameObject shieldBar;
    public GameObject boss;

    public SpeedBoosterTrigger lastBoosterTrigger;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startRocketPos = transform.position;
    }

    public void EnableRB()
    {
        rb.isKinematic = false;
    }

    public void SetRocketVariables()
    {
        currentFuel = startFuel;
        currentShield = startShield;
        if (currentShield == 0)
        {
            shieldBar.SetActive(false);
        }
        else
        {
            shieldBar.SetActive(true);

        }
        lowFuelIndicationValue = startFuel * lowFuelTargetPercent / 100;
        playerFuelBar.fillAmount = 1;
        playerShieldBar.fillAmount = 1;
        oneHitFuelBarFill = playerFuelBar.fillAmount / startFuel;
        oneHitShieldBarFill = playerShieldBar.fillAmount / startShield;
        fuelDecreaseMultiplier = 2;
        cineCamera.m_Lens.FieldOfView = 60;


        shieldBarText.text = $"{currentShield} / {startShield}";
    }

    private void FixedUpdate()
    {
        playerFuelBar.fillAmount = oneHitFuelBarFill * currentFuel;
        playerShieldBar.fillAmount = oneHitShieldBarFill * currentShield;

        playerFuelBar.color = fuelGraident.Evaluate(playerFuelBar.fillAmount);

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
            Instantiate(destroyShieldParticlePrefab, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            shieldParticle.SetActive(false);

            if (currentFuel > 0)
            {
                currentFuel -= value;
                playerFuelBar.fillAmount = oneHitFuelBarFill * currentFuel;
            }
            /*else if(currentFuel <= 0)
            {
                CreateHighScoreLine();
                Dead();
            }*/
        }
        else
        {
            if (currentShield <= 0 || currentShield - value <= 0)
            {
                Instantiate(destroyShieldParticlePrefab, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
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
        LevelProgress.Instance.ShowDamageIndication();
        SFX.Instance.PlayHitSound(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Dead();
        }
    }

    [ContextMenu("Test Dead")]
    public void Dead()
    {
        if (isDead)
            return;
        isDead = true;
        Debug.Log("dead");
        CreateHighScoreLine();
        LevelProgress.Instance.CancelFillTween();
        playerFuelBar.gameObject.SetActive(false);
        shieldBarText.gameObject.SetActive(false);
        playerShieldBar.gameObject.SetActive(false);
        cineCamera.LookAt = null;
        cineCamera.Follow = null;
        rocketContainer.GetComponent<PlanetsMovement>().enabled = true;
        SpaceShipMovement.Instance.playerCanControl = false;
        StartCoroutine(TurnOffSpeed());
        GameStateHandler.Instance.DisableGuns();
        GameStateHandler.Instance.ResetEnginesCount();
        LevelsHandler.Instance.SlowlyStopMoving();
    }

    private IEnumerator TurnOffSpeed()
    {
        yield return new WaitForSeconds(0.5f);
        SpaceShipMovement.Instance.SetZeroVariables();
        yield return new WaitForSeconds(1f);

        loseScreen.SetActive(true);
        yield return new WaitForSeconds(2.2f);
        rocketContainer.GetComponent<PlanetsMovement>().enabled = false;
        cineCamera.LookAt = transform;
        cineCamera.Follow = transform;
        SpaceShipMovement.Instance.playerCanControl = true;
        this.enabled = false;
    }

    [ContextMenu("Restart Rocket")]
    public void RestartRocket()
    {
        currentFuel = startFuel;
        currentShield = startShield;
        rocketContainer.localPosition = Vector3.zero;
        rocketContainer.localEulerAngles = Vector3.zero;
        playerFuelBar.gameObject.SetActive(true);
        shieldBarText.gameObject.SetActive(true);
        playerShieldBar.gameObject.SetActive(true);
        transform.localPosition = new Vector3(4, 5, 0);
        GameStateHandler.Instance.UnCenterRocket();
        rb.isKinematic = true;
        loseScreen.SetActive(false);
        DisableLowFuelIndicator();
        boss.SetActive(false);
        isDead = false;
        isPlayerOnPlanet = false;
        SpaceShipMovement.Instance.enabled = false;
        BoostersManager.Instance.ClearBoosterList();
        LevelsHandler.Instance.SetPropsContainerPositionToNormal();
        this.enabled = false;
    }

    public void DisableLowFuelIndicator()
    {
        lowFuelIndication.SetActive(false);
        lowFuelEnabled = false;
    }

    public void CreateHighScoreLine()
    {
        LevelProgress.Instance.SetHighSCoreLine();
    }
}