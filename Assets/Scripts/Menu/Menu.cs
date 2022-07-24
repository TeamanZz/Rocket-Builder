using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu Instance;

    [SerializeField] private GameObject blackScreenPrefab;

    public List<GameObject> previewScreenToBuildEnable = new List<GameObject>();
    public List<GameObject> previewScreenToBuildDisable = new List<GameObject>();

    public List<GameObject> endScreenToPreviewEnable = new List<GameObject>();
    public List<GameObject> endScreenToPreviewDisable = new List<GameObject>();

    public List<GameObject> objectsToEnableFly = new List<GameObject>();
    public List<GameObject> objectsToDisableFly = new List<GameObject>();

    public List<RandomSpawner> triggers = new List<RandomSpawner>();
    public Transform enemiesContainer;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetAllTriggers()
    {
        foreach (var item in triggers)
        {
            item.wasSpawned = false;
        }
    }

    public void DestroyAllActiveEnemies()
    {
        for (var i = enemiesContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(enemiesContainer.GetChild(i).gameObject);
        }
    }

    public void ActivateBuildScreen()
    {
        for (int i = 0; i < previewScreenToBuildEnable.Count; i++)
        {
            previewScreenToBuildEnable[i].SetActive(true);
        }

        for (int i = 0; i < previewScreenToBuildDisable.Count; i++)
        {
            previewScreenToBuildDisable[i].SetActive(false);
        }
    }

    public void ActivatePreviewScreen()
    {
        for (int i = 0; i < endScreenToPreviewEnable.Count; i++)
        {
            endScreenToPreviewEnable[i].SetActive(true);
        }

        for (int i = 0; i < endScreenToPreviewDisable.Count; i++)
        {
            endScreenToPreviewDisable[i].SetActive(false);
        }
    }
}