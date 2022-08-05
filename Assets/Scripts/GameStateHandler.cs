using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public static GameStateHandler Instance;

    public float rocketConstantVelocity = 3f;
    public float rocketRotationDamping = 7f;
    public int rocketSideSpeed = 3;

    [Header("EndBuild")]
    [SerializeField] private Transform startRocketPosition;
    [SerializeField] private GameObject blackScreenPrefab;
    [SerializeField] private GameObject uiItems;
    [SerializeField] private Transform rocketObject;
    [SerializeField] private Transform rocketContainer;

    [Header("ShipSettings")]
    private ResourcesHandler resourcesHandler;
    private SpaceShipMovement playerShipMovement;

    public BuildingGrid buildingGrid;
    public CameraBoundsController boundsController;
    public GameObject cantFlyButton;

    [Header("ItemsToActive")]
    [SerializeField] private GameObject[] itemsToActive;
    [Header("ItemsToUnActive")]
    [SerializeField] private GameObject[] itemsToUnActive;

    private Vector3 lastCapsulePosition;

    private void Awake()
    {
        Instance = this;
        resourcesHandler = FindObjectOfType<ResourcesHandler>();
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

    public void DisableGuns()
    {
        var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
        {
            gun.GetComponent<IShootable>().ForbidShoot();
        }
    }

    public void SetShipSettings()
    {
        var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
        {
            gun.GetComponent<IShootable>().AllowShoot();
        }

        // if (buildingGrid.placedItems.Find(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Shield))
        //     PlayerRocket.Instance.shieldParticle.SetActive(true);

        PlayerRocket.Instance.enabled = true;
        boundsController.enabled = true;

        PlayerRocket.Instance.startFuel = resourcesHandler.trueFuelValue;
        PlayerRocket.Instance.startShield = resourcesHandler.trueShieldValue;
        PlayerRocket.Instance.currentFuel = resourcesHandler.trueFuelValue;
        PlayerRocket.Instance.currentShield = resourcesHandler.trueShieldValue;
        playerShipMovement.SetNewValues(resourcesHandler.trueMoveSpeedValue);
        ScreenShake.Instance.ShakeScreenOnRocketStart();
        PlayerRocket.Instance.SetRocketVariables();
        LevelProgress.Instance.ResetHeightVariables();
        Menu.Instance.SetNewTriggers(LevelsHandler.Instance.GetCurrentTriggers());
        Menu.Instance.SetNewLootTriggers(LevelsHandler.Instance.GetCurrentLootTriggers());
        for (int i = 0; i < itemsToActive.Length; i++)
        {
            itemsToActive[i].SetActive(true);
        }
        for (int i = 0; i < itemsToUnActive.Length; i++)
        {
            itemsToUnActive[i].SetActive(false);
        }
    }

    private void CenterRocket()
    {
        for (var i = rocketContainer.transform.childCount - 1; i > 0; i--)
        {
            rocketContainer.transform.GetChild(i).SetParent(buildingGrid.startCapsule.transform);
        }

        lastCapsulePosition = buildingGrid.startCapsule.transform.localPosition;
        buildingGrid.startCapsule.transform.localPosition = Vector3.zero;

        for (var i = buildingGrid.startCapsule.transform.childCount - 1; i > 0; i--)
        {
            buildingGrid.startCapsule.transform.GetChild(i).SetParent(rocketContainer);
        }
    }

    public void UnCenterRocket()
    {
        for (var i = rocketContainer.transform.childCount - 1; i > 0; i--)
        {
            rocketContainer.transform.GetChild(i).SetParent(buildingGrid.startCapsule.transform);
        }

        buildingGrid.startCapsule.transform.localPosition = lastCapsulePosition;

        for (var i = buildingGrid.startCapsule.transform.childCount - 1; i > 0; i--)
        {
            buildingGrid.startCapsule.transform.GetChild(i).SetParent(rocketContainer);
        }
    }

    private void SetNewRocketContainerOffset()
    {
        float minDistance = Mathf.Infinity;
        float maxDistance = Mathf.NegativeInfinity;

        for (int i = 0; i < buildingGrid.placedItems.Count; i++)
        {
            if (buildingGrid.placedItems[i].transform.position.y < minDistance)
            {
                minDistance = buildingGrid.placedItems[i].transform.position.y;
            }
        }

        for (int i = 0; i < buildingGrid.placedItems.Count; i++)
        {
            if (buildingGrid.placedItems[i].transform.position.y > maxDistance)
            {
                maxDistance = buildingGrid.placedItems[i].transform.position.y;
            }
        }

        for (var i = buildingGrid.placedItems.Count - 1; i >= 0; i--)
        {
            buildingGrid.placedItems[i].transform.parent = null;
        }
        var newYValue = (minDistance + maxDistance) / 2;
        rocketContainer.transform.position = new Vector3(rocketContainer.transform.position.x, newYValue, rocketContainer.transform.position.z);
        for (var i = buildingGrid.placedItems.Count - 1; i >= 0; i--)
        {
            buildingGrid.placedItems[i].transform.parent = rocketContainer;
        }
    }

    public IEnumerator EndBuildingCoroutine()
    {
        PlayerRocket.Instance.EnableRB();
        var blackScreen = Instantiate(blackScreenPrefab);
        uiItems.SetActive(false);
        buildingGrid.CheckOnDisconnectedParts();
        buildingGrid.DisableItemsConnectors();
        yield return new WaitForSeconds(1f);
        SetShipSettings();
        Destroy(blackScreen);
        CenterRocket();
        SetNewRocketContainerOffset();
        LevelProgress.Instance.StartHeightBarFilling();
        rocketObject.transform.position = startRocketPosition.position;
        yield return new WaitForSeconds(1f);
        LevelsHandler.Instance.EnableCurrentContainerBackgroundMovement();
    }
}