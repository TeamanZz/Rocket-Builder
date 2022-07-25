using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private GameObject[] thingsToSetFalse;
    [SerializeField] private GameObject secondCamera;
    [SerializeField] private GameObject blackScreenPrefab, winScreenPrefab;
    [SerializeField] private Transform startPos, finalPos;
    [SerializeField] private BuildingGrid buildingGrid;
    [SerializeField] private GameObject buildings;
    [SerializeField] private GameObject rotatedCamera;

    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            StartScene();
        }
    }

    [ContextMenu("Win")]
    public void StartScene()
    {
        if (PlayerRocket.Instance.shieldParticle.activeSelf)
        {
            PlayerRocket.Instance.shieldParticle.SetActive(false);
        }
        var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
        {
            gun.GetComponent<Gun>().ForbidShoot();
        }
        LevelProgress.Instance.CancelFillTween();
        PlayerRocket.Instance.isPlayerOnPlanet = true;
        for (int i = 0; i < thingsToSetFalse.Length; i++)
        {
            thingsToSetFalse[i].SetActive(false);
        }

        gameObject.GetComponent<BoxCollider>().enabled = false;
        secondCamera.SetActive(true);
        StartCoroutine(FinalScene());
    }

    public IEnumerator FinalScene()
    {
        PlayerRocket.Instance.DisableLowFuelIndicator();
        PlayerRocket.Instance.GetComponent<SpaceShipMovement>().constantVelocity = 0;
        PlayerRocket.Instance.transform.position = startPos.position;
        Debug.Log("PlayerShipOnStartPlace");
        var screen = Instantiate(blackScreenPrefab);
        yield return new WaitForSeconds(2f);
        rotatedCamera.transform.DORotate(new Vector3(0, 0, 0), 10f);
        Destroy(screen);
        buildings.SetActive(true);
        PlayerRocket.Instance.transform.DOMove(finalPos.position, 3f).SetEase(Ease.Linear);
        Debug.Log("PlayerShipOnFinalPlace");
        yield return new WaitForSeconds(3f);
        Instantiate(winScreenPrefab);
    }
}
