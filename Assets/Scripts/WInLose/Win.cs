using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private GameObject[] thingsToSetFalse;
    [SerializeField] private List<GameObject> thingsToSetTrueAfterWin = new List<GameObject>();
    [SerializeField] private GameObject secondCamera;
    [SerializeField] private Transform startPos, finalPos;
    [SerializeField] private BuildingGrid buildingGrid;
    [SerializeField] private GameObject buildings;
    [SerializeField] private GameObject rotatedCamera;
    [SerializeField] private Vector3 rotatedCameraRotation;

    public WinPanel winPanel;

    private Tween rotateTween;
    private Tween moveTween;

    private void Start()
    {
        rotatedCameraRotation = rotatedCamera.transform.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            if (PlayerRocket.Instance.isDead || PlayerRocket.Instance.isPlayerOnPlanet)
                return;
            StartScene();
        }
    }

    [ContextMenu("Win")]
    public void StartScene()
    {
        winPanel.gameObject.SetActive(true);
        winPanel.PlayFade();

        Menu.Instance.DestroyAllActiveEnemies();
        EnemyManager.Instance.ClearEnemyList();

        if (PlayerRocket.Instance.shieldParticle.activeSelf)
            PlayerRocket.Instance.shieldParticle.SetActive(false);

        var gunsList = buildingGrid.placedItems.FindAll(x => x.isMainRocketPiece && x.itemType == BuildItem.ItemType.Weapon);
        foreach (var gun in gunsList)
            gun.GetComponent<IShootable>().ForbidShoot();

        LevelProgress.Instance.CancelFillTween();
        PlayerRocket.Instance.isPlayerOnPlanet = true;
        StartCoroutine(FinalScene());
    }

    public IEnumerator FinalScene()
    {
        yield return new WaitForSeconds(1f);

        PlayerRocket.Instance.DisableLowFuelIndicator();
        PlayerRocket.Instance.GetComponent<SpaceShipMovement>().constantVelocity = 0;
        PlayerRocket.Instance.transform.position = startPos.position;
        moveTween = PlayerRocket.Instance.transform.DOMove(finalPos.position, 3f).SetEase(Ease.OutBack);
        rotateTween = rotatedCamera.transform.DORotate(new Vector3(0, 0, 0), 10f);

        secondCamera.SetActive(true);
        buildings.SetActive(true);

        for (int i = 0; i < thingsToSetFalse.Length; i++)
            thingsToSetFalse[i].SetActive(false);
    }

    public void HandleContinueButton()
    {
        Menu.Instance.ResetAllTriggers();
        Menu.Instance.ActivatePreviewScreen();
        LevelsHandler.Instance.SetNewLevel();
        PlayerRocket.Instance.RestartRocket();
        secondCamera.SetActive(false);
        buildings.SetActive(false);
        rotateTween.Kill();
        moveTween.Kill();
        for (int i = 0; i < thingsToSetTrueAfterWin.Count; i++)
            thingsToSetTrueAfterWin[i].SetActive(true);
        rotatedCamera.transform.eulerAngles = rotatedCameraRotation;

        winPanel.gameObject.SetActive(false);
    }
}