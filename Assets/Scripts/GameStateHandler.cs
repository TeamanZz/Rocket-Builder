using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public static GameStateHandler Instance;
    [Header("EndBuild")]
    [SerializeField] private Transform startRocketPosition;
    [SerializeField] private GameObject blackScreenPrefab;
    [SerializeField] private GameObject uiItems;
    [SerializeField] private Transform rocketObject;
    [SerializeField] private Transform rocketContainer;

    [Header("ItemsToActive")]
    [SerializeField] private GameObject[] itemsToActive;
    [Header("ItemsToUnActive")]
    [SerializeField] private GameObject[] itemsToUnActive;

    [Header("ShipSettings")]
    [SerializeField] private ResourcesHandler resourcesHandler;
    [SerializeField] private EnemyBase playerShipHealth;
    [SerializeField] private SpaceShipMovement playerShipMovement;


    public BuildingGrid buildingGrid;

    public GameObject cantFlyButton;

    private void Awake()
    {
        Instance = this;
        resourcesHandler = FindObjectOfType<ResourcesHandler>();
        playerShipHealth = GameObject.FindWithTag("Player").GetComponent<EnemyBase>();
        playerShipMovement = FindObjectOfType<SpaceShipMovement>();
    }

    public void EnableFlightButton()
    {
        cantFlyButton.SetActive(false);
    }

    public void DisableFlightButton()
    {
        cantFlyButton.SetActive(true);
    }

    public void EndBuidling()
    {
        StartCoroutine(EndBuildingCoroutine());
    }

    public void SetShipSettings()
    {
        var gunsList =
            buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
        {
            gun.GetComponent<Gun>().AllowShoot();
        }
        playerShipHealth.enabled = true;
        playerShipHealth.startFuel = resourcesHandler.trueFuelValue;
        playerShipHealth.startShield = resourcesHandler.trueShieldValue;
        for (int i = 0; i < itemsToActive.Length; i++)
        {
            itemsToActive[i].SetActive(true);
        }
        for (int i = 0; i < itemsToUnActive.Length; i++)
        {
            itemsToUnActive[i].SetActive(false);
        }
        playerShipMovement.constantVelocity = 3f;
        playerShipMovement.rotationDamping = 7f;
        playerShipMovement.sideSpeed = 3;
    }

    private void CenterRocket()
    {
        for (var i = rocketContainer.transform.childCount - 1; i > 0; i--)
        {
            rocketContainer.transform.GetChild(i).SetParent(buildingGrid.startCapsule.transform);
        }

        buildingGrid.startCapsule.transform.localPosition = Vector3.zero;

        for (var i = buildingGrid.startCapsule.transform.childCount - 1; i > 0; i--)
        {
            buildingGrid.startCapsule.transform.GetChild(i).SetParent(rocketContainer);
        }
    }

    public IEnumerator EndBuildingCoroutine()
    {
        var blackScreen = Instantiate(blackScreenPrefab);
        uiItems.SetActive(false);
        buildingGrid.CheckOnDisconnectedParts();
        buildingGrid.ToggleItemsConnectors();
        yield return new WaitForSeconds(1f);
        SetShipSettings();
        Destroy(blackScreen);
        CenterRocket();
        rocketObject.transform.position = startRocketPosition.position;
    }
}