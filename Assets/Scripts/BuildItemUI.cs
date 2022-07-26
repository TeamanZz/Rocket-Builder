using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BuildItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BuildItem buildItemPrefab;
    public PartsUIItem partsUIItem;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (partsUIItem.countValue == 0 || !partsUIItem.isBoughted)
            return;
        BuildingGrid.Instance.StartPlacingNewItem(buildItemPrefab, partsUIItem);
        transform.DOPunchRotation(Vector3.forward * 7, 0.5f);
        transform.DOLocalMoveY(transform.localPosition.y + 30, 0.3f).SetEase(Ease.InOutBack).SetLoops(2, LoopType.Yoyo);
        partsUIItem.DescreaseCount();
        // ResourcesHandler.Instance.DecreaseFuelValue(partsUIItem.statValue);
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (BuildingGrid.Instance.placingItem != null)
            BuildingGrid.Instance.HandleDropItem();
    }
}