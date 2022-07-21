using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPartsHandler : MonoBehaviour
{
    public int lastUnlockedEventIndex = 0;
    public List<UnlockableEvent> unlockEvents = new List<UnlockableEvent>();

    [ContextMenu("UnlockNextEvent")]
    public void UnlockNextEvent()
    {
        if (lastUnlockedEventIndex > unlockEvents.Count - 1)
            return;

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