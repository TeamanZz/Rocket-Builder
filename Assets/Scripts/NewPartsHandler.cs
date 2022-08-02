using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPartsHandler : MonoBehaviour
{
    public static NewPartsHandler Instance;

    public int lastUnlockedEventIndex = 0;
    public List<UnlockableEvent> unlockEvents = new List<UnlockableEvent>();
    public GameObject shieldsListButton;

    public GameObject rocketPartPopup;

    public List<PartsUIItem> boughtedItems = new List<PartsUIItem>();

    private void Awake()
    {
        Instance = this;
    }
    [ContextMenu("UnlockNewDuplicate")]
    public void UnlockNewDuplicate()
    {
        rocketPartPopup.SetActive(true);

        int itemIndex = Random.Range(1, boughtedItems.Count);
        boughtedItems[itemIndex].IncreaseCount();
    }

    [ContextMenu("UnlockNextEvent")]
    public void UnlockNextEvent()
    {
        if (lastUnlockedEventIndex > unlockEvents.Count - 1)
            return;

        if (lastUnlockedEventIndex == 0)
            shieldsListButton.SetActive(true);

        unlockEvents[lastUnlockedEventIndex].UnlockThisEvent();
        lastUnlockedEventIndex++;
    }

    public void RemoveAllDuplicates()
    {
        for (var i = 0; i < boughtedItems.Count; i++)
        {
            if (boughtedItems[i].mainImage.GetComponent<BuildItemUI>().buildItemPrefab.itemType == BuildItem.ItemType.Capsule)
                boughtedItems[i].countValue = 0;
            else
            {
                boughtedItems[i].countValue = 1;
            }
            boughtedItems[i].HandleView();
        }
    }
}

[System.Serializable]
public class UnlockableEvent
{
    public EventType eventType;
    public PartsUIItem buildItem;

    public void UnlockThisEvent()
    {
        if (eventType == EventType.IncreaseItemCount)
        {
            buildItem.IncreaseCount();
        }
        else if (eventType == EventType.UnlockItem)
        {
            buildItem.gameObject.SetActive(true);
        }
    }

    public enum EventType
    {
        UnlockItem,
        IncreaseItemCount
    }
}