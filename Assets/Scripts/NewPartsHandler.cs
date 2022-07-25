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
        Debug.Log(itemIndex);
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