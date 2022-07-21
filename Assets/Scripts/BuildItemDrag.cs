using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        BuildItem dragObject = GetComponent<BuildItem>();
        BuildingGrid.Instance.StartPlacingExistItem(dragObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BuildingGrid.Instance.HandleDropItem();
    }

    public void OnDrag(PointerEventData eventData) { }
}