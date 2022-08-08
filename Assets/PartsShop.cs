using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PartsShop : MonoBehaviour
{
    public static PartsShop Instance;

    public Money money;
    public List<CanvasGroup> buyButtonsList = new List<CanvasGroup>();
    public List<int> pricesList = new List<int>();
    public List<TextMeshProUGUI> pricesTextList = new List<TextMeshProUGUI>();
    public List<GameObject> rocketItemsPrefabs = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdatePricesView();
    }

    public void UpdatePricesView()
    {
        for (int i = 0; i < pricesList.Count; i++)
        {
            pricesTextList[i].text = pricesList[i].ToString();
            if (!money.IsEnoughCurrency(pricesList[i]))
            {
                buyButtonsList[i].alpha = 0.5f;
            }
            else
            {
                buyButtonsList[i].alpha = 1;
            }
        }
    }

    public void BuyRocketItem(int itemIndex)
    {
        if (!money.IsEnoughCurrency(pricesList[itemIndex]))
            return;

        money.DecreaseCurrency(pricesList[itemIndex]);
        pricesList[itemIndex] = (int)(pricesList[itemIndex] * 1.3f);

        Vector2Int? freeGridPlace = BuildingGrid.Instance.GetFreePlace();
        Vector3 newPlace = new Vector3(freeGridPlace.Value.x, freeGridPlace.Value.y, 0);
        var newPart = Instantiate(rocketItemsPrefabs[itemIndex], newPlace, Quaternion.identity, PlayerRocket.Instance.rocketContainer);
        var buildItemComponent = newPart.GetComponent<BuildItem>();
        BuildingGrid.Instance.placingItem = buildItemComponent;
        BuildingGrid.Instance.currentPlacingItemPosition = new Vector2(freeGridPlace.Value.x, freeGridPlace.Value.y);
        BuildingGrid.Instance.HandleDropItem();
        newPart.transform.localScale = Vector3.zero;
        newPart.transform.DOScale(Vector3.one * 0.5f, 0.3f).SetEase(Ease.OutBack);
        UpdatePricesView();
    }

    public void PlaceUpgradedRocketItem(int itemIndex, int placeX, int placeY)
    {
        Debug.Log("item index =" + itemIndex);
        Vector3 newPlace = new Vector3(placeX, placeY, 0);
        var newPart = Instantiate(rocketItemsPrefabs[itemIndex], newPlace, Quaternion.identity, PlayerRocket.Instance.rocketContainer);
        var buildItemComponent = newPart.GetComponent<BuildItem>();
        BuildingGrid.Instance.placingItem = buildItemComponent;
        BuildingGrid.Instance.currentPlacingItemPosition = new Vector2(newPlace.x, newPlace.y);
        BuildingGrid.Instance.HandleDropItem();
        newPart.transform.localScale = Vector3.zero;
        newPart.transform.DOScale(Vector3.one * 0.5f, 0.3f).SetEase(Ease.OutBack);
    }
}