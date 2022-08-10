using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesHandler : MonoBehaviour
{
    public ItemsListHandler itemsListHandler;

    private void Start()
    {
        LoadItemsData();
    }

    private void LoadItemsData()
    {
        for (int i = 0; i < itemsListHandler.partsContainers.Count; i++)
        {
            for (int k = 0; k < itemsListHandler.partsContainers[i].transform.childCount; k++)
            {
                var itemComponent = itemsListHandler.partsContainers[i].transform.GetChild(k).GetComponent<PartsUIItem>();
                // itemComponent.
                int count = PlayerPrefs.GetInt($"{itemComponent.index} Item Count", itemComponent.countValue);
                itemComponent.countValue = count;

                int tempBoughted = 0;
                if (itemComponent.isBoughted)
                    tempBoughted = 1;
                else
                    tempBoughted = 0;

                int isBoughted = PlayerPrefs.GetInt($"{itemComponent.index} Item IsBoughted", tempBoughted);

                if (isBoughted == 0)
                    itemComponent.isBoughted = false;
                else if (isBoughted == 1)
                    itemComponent.isBoughted = true;

                itemComponent.HandleView();
            }
        }
    }

    [ContextMenu("ResetPrefsExceptTutorial")]
    public void ResetPrefsExceptTutorial()
    {
        PlayerPrefs.DeleteKey("MoneyCount");

        for (int i = 0; i < 12; i++)
        {
            PlayerPrefs.DeleteKey($"{i} Item Count");
            PlayerPrefs.DeleteKey($"{i} Item IsBoughted");
        }
    }

    [ContextMenu("ResetAllPrefs")]
    public void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("ResetTutorial")]
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialDone");
    }
}