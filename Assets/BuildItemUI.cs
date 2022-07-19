using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BuildItem buildItemPrefab;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("ondrag1");
        BuildingGrid.Instance.StartPlacingItem(buildItemPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("IEnd");
        BuildingGrid.Instance.HandleDropItem();
    }
}