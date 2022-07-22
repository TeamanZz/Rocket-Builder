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
    private ResourcesHandler resourcesHandler;
    private SpaceShipMovement playerShipMovement;
    private PlayerRocket playerRocket;

    public BuildingGrid buildingGrid;

    public GameObject cantFlyButton;

    private void Awake()
    {
        Instance = this;
        playerRocket = FindObjectOfType<PlayerRocket>();
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

    public void SetShipSettings()
    {
        var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
        {
            gun.GetComponent<Gun>().AllowShoot();
        }

        var shieldList =
            buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Shield);
        foreach (var shield in shieldList)
        {
            PlayerRocket.Instance.shieldParticle.SetActive(true);
        }

        PlayerRocket.Instance.enabled = true;
        PlayerRocket.Instance.startFuel = resourcesHandler.trueFuelValue;
        PlayerRocket.Instance.startShield = resourcesHandler.trueShieldValue;
        PlayerRocket.Instance.currentFuel = resourcesHandler.trueFuelValue;
        PlayerRocket.Instance.currentShield = resourcesHandler.trueShieldValue;
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

    private void SetNewRocketContainerOffset()
    {
        float minDistance = Mathf.Infinity;
        float maxDistance = Mathf.NegativeInfinity;

        for (int i = 0; i < buildingGrid.placedItems.Count; i++)
        {
            if (buildingGrid.placedItems[i].transform.position.y < minDistance)
            {
                Debug.Log("New min");
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
        Debug.Log(minDistance);

        for (var i = buildingGrid.placedItems.Count - 1; i >= 0; i--)
        {
            buildingGrid.placedItems[i].transform.parent = null;
        }
        Debug.Log("min =" + minDistance + "max =" + maxDistance);
        var newYValue = (minDistance + maxDistance) / 2;
        rocketContainer.transform.position = new Vector3(rocketContainer.transform.position.x, newYValue, rocketContainer.transform.position.z);
        for (var i = buildingGrid.placedItems.Count - 1; i >= 0; i--)
        {
            buildingGrid.placedItems[i].transform.parent = rocketContainer;
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
        SetNewRocketContainerOffset();
        rocketObject.transform.position = startRocketPosition.position;
    }
}