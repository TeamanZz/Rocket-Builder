using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BuildItem buildItemPrefab;
    public void OnBeginDrag(PointerEventData eventData)
    {
        BuildingGrid.Instance.StartPlacingNewItem(buildItemPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BuildingGrid.Instance.HandleDropItem();
    }
}