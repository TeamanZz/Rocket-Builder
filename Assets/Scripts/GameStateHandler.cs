using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    [Header("EndBuild")]
    [SerializeField] private Transform startRocketPosition;
    [SerializeField] private Camera rocketCamera;
    [SerializeField] private GameObject blackScreenPrefab;
    [SerializeField] private GameObject uiItems;
    [SerializeField] private Transform rocketObject;

    public BuildingGrid buildingGrid;

    public void EndBuidling()
    {
        StartCoroutine(EndBuildingCoroutine());
    }

    private void CenterRocket()
    {
        // for (var i = 1; i < rocketObject.transform.childCount; i++)
        // {
        //     rocketObject.transform.GetChild(i).SetParent(buildingGrid.startCapsule.transform);
        // }

        // buildingGrid.startCapsule.transform.parent = null;
        // buildingGrid.startCapsule.transform.position = startRocketPosition.position;
        // buildingGrid.startCapsule.transform.parent = rocketObject;

        // for (var i = 1; i < rocketObject.transform.childCount; i++)
        // {
        //     rocketObject.transform.GetChild(i).SetParent(rocketObject);
        // }
    }

    public IEnumerator EndBuildingCoroutine()
    {
        var blackScreen = Instantiate(blackScreenPrefab);
        uiItems.SetActive(false);
        buildingGrid.CheckOnDisconnectedParts();
        buildingGrid.ToggleItemsConnectors();
        yield return new WaitForSeconds(1f);
        Destroy(blackScreen);
        CenterRocket();
        rocketObject.transform.position = startRocketPosition.position;
        rocketCamera.gameObject.SetActive(true);
        Camera.main.gameObject.SetActive(false);
    }
}