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

    public IEnumerator EndBuildingCoroutine()
    {
        var blackScreen = Instantiate(blackScreenPrefab);
        uiItems.SetActive(false);
        buildingGrid.CheckOnDisconnectedParts();
        buildingGrid.ToggleItemsConnectors();
        yield return new WaitForSeconds(1f);
        Destroy(blackScreen);
        rocketObject.transform.position = startRocketPosition.position;
        rocketCamera.gameObject.SetActive(true);
        Camera.main.gameObject.SetActive(false);
    }
}