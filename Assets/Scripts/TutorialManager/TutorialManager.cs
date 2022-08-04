using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private GameObject tapToBuildObject;
    [SerializeField] private GameObject dragEngineObject;
    [SerializeField] private GameObject tapToFuelPartsObject;
    [SerializeField] private GameObject dragFuelObject;
    [SerializeField] private GameObject tapToPlayObject;
    [SerializeField] private GameObject lootItemTutorialObject;

    [Header("OtherButtons")] 
    [SerializeField] private Button gunButton;
    [SerializeField] private Button shieldButton;
    [SerializeField] private Button fuelButton;
    [SerializeField] private Button engineButton;
    [SerializeField] private Button resetAll;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            gunButton.enabled = true;
            shieldButton.enabled = true;
            fuelButton.enabled = true;
            engineButton.enabled = true;
            resetAll.enabled = true;
        }
        else
        {
            gunButton.enabled = false;
            shieldButton.enabled = false;
            fuelButton.enabled = false;
            engineButton.enabled = false;
            resetAll.enabled = false;
        }
        
        ShowTapToBuildTutorial();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Prefs deleted");
        }
    }

    public void ShowTapToBuildTutorial()
    {
        
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            tapToBuildObject.SetActive(false);
        }
        else
        {
            tapToBuildObject.SetActive(true);
        }
        
    }

    public void ShowDragEngineTutorial()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            dragEngineObject.SetActive(false);
        }
        else
        {
            engineButton.enabled = true;
            fuelButton.enabled = false;
            tapToBuildObject.SetActive(false);
            dragEngineObject.SetActive(true);
        }
    }

    public void EnableFuelButton()
    {
        fuelButton.enabled = true;
        engineButton.enabled = false;
    }

    public void DisableFuelButton()
    {
        fuelButton.enabled = false;
    }

    public void ShowDragFuelTutorial()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            tapToFuelPartsObject.SetActive(false);
        }
        else
        {
            tapToFuelPartsObject.SetActive(false);
            dragFuelObject.SetActive(true);
        }
    }

    public void ShowTapToPlayTutorial()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            tapToPlayObject.SetActive(false);
        }
        else
        {
            dragFuelObject.SetActive(false);
            tapToPlayObject.SetActive(true);
        }
    }

    public void ShowLootItemTutorial()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            return;
        }
        else
        {
            PlayerPrefs.SetInt("TutorialDone",1);
            lootItemTutorialObject.SetActive(true);
            SpaceShipMovement.Instance.gameObject.transform.DOMove(
                new Vector3(3.5f, SpaceShipMovement.Instance.gameObject.transform.position.y,
                    SpaceShipMovement.Instance.gameObject.transform.position.z), 1f);
            SpaceShipMovement.Instance.constantVelocity = 0;
            SpaceShipMovement.Instance.sideSpeed = 0;
            PlayerRocket.Instance.fuelDecreaseMultiplier = 0;
            LevelProgress.Instance.StopFillingHeightBar();
            StartCoroutine(ContinuePlayingAfterLootTutorial());
        }
    }

    public IEnumerator ContinuePlayingAfterLootTutorial()
    {
        yield return new WaitForSeconds(4f);
        LevelProgress.Instance.ContinueFillingHeightBar();
        lootItemTutorialObject.SetActive(false);
        SpaceShipMovement.Instance.constantVelocity = SpaceShipMovement.Instance.defaultConstantVelocity;
        SpaceShipMovement.Instance.sideSpeed = SpaceShipMovement.Instance.defaultSideSpeed;
        PlayerRocket.Instance.fuelDecreaseMultiplier = 2;
    }

    public void DestroyAllTutorials()
    {
        tapToBuildObject.SetActive(false);
        dragEngineObject.SetActive(false);
        tapToFuelPartsObject.SetActive(false);
        dragFuelObject.SetActive(false);
        tapToPlayObject.SetActive(false);
        gunButton.enabled = true;
        shieldButton.enabled = true;
        fuelButton.enabled = true;
        engineButton.enabled = true;
        resetAll.enabled = true;
    }
}
