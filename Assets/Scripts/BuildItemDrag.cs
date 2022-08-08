using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BuildItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 startDragPos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPos = transform.position;
        BuildItem dragObject = GetComponent<BuildItem>();
        BuildingGrid.Instance.StartPlacingExistItem(dragObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BuildingGrid.Instance.HandleDropItem();
    }

    public void OnDrag(PointerEventData eventData) { }

    public void ResetPosition()
    {
        transform.DOMove(startDragPos, 0.5f);
    }
}