using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private GameObject[] thingsToSetFalse;
    [SerializeField] private GameObject secondCamera;
    [SerializeField] private Transform startPos, finalPos;
    [SerializeField] private BuildingGrid buildingGrid;
    [SerializeField] private GameObject buildings;
    [SerializeField] private GameObject rotatedCamera;

    public WinPanel winPanel;

    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            if (PlayerRocket.Instance.isDead)
                return;
            StartScene();
        }
    }

    [ContextMenu("Win")]
    public void StartScene()
    {
        winPanel.gameObject.SetActive(true);
        winPanel.PlayFade();
        if (PlayerRocket.Instance.shieldParticle.activeSelf)
            PlayerRocket.Instance.shieldParticle.SetActive(false);

        var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
            gun.GetComponent<Gun>().ForbidShoot();

        LevelProgress.Instance.CancelFillTween();
        PlayerRocket.Instance.isPlayerOnPlanet = true;


        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(FinalScene());
    }

    public IEnumerator FinalScene()
    {
        yield return new WaitForSeconds(1f);
        PlayerRocket.Instance.DisableLowFuelIndicator();
        PlayerRocket.Instance.GetComponent<SpaceShipMovement>().constantVelocity = 0;
        PlayerRocket.Instance.transform.position = startPos.position;
        for (int i = 0; i < thingsToSetFalse.Length; i++)
        {
            thingsToSetFalse[i].SetActive(false);
        }
        secondCamera.SetActive(true);
        rotatedCamera.transform.DORotate(new Vector3(0, 0, 0), 10f);
        buildings.SetActive(true);
        PlayerRocket.Instance.transform.DOMove(finalPos.position, 3f).SetEase(Ease.Linear);
    }
}