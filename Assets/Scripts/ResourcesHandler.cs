using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResourcesHandler : MonoBehaviour
{
    public static ResourcesHandler Instance;

    public float trueFuelValue;
    public float trueShieldValue;
    public float trueMoveSpeedValue;

    private float fuelValueView;
    private float shieldValueView;


    [SerializeField] private TextMeshProUGUI fuelValueText;
    [SerializeField] private TextMeshProUGUI shieldValueText;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        fuelValueText.text = "Fuel - " + fuelValueView.ToString();
        shieldValueText.text = "Shield - " + shieldValueView.ToString();
    }

    public void SetNewMoveSpeedValue(float newValue)
    {
        trueMoveSpeedValue = newValue;
    }

    public void SetNewFuelValue(int newFuelvalue)
    {
        trueFuelValue = newFuelvalue;
        DOTween.To(() => fuelValueView, x => fuelValueView = (int)x, trueFuelValue, 1);
    }

    public void SetNewShieldValue(int newShieldValue)
    {
        trueShieldValue = newShieldValue;
        DOTween.To(() => shieldValueView, x => shieldValueView = (int)x, trueShieldValue, 1);
    }

    public void HandleStatsIncrease(BuildItem buildItem)
    {
        if (buildItem.itemType == BuildItem.ItemType.Fuel)
        {
            trueFuelValue += buildItem.placingItemUI.statValue;
        }
        if (buildItem.itemType == BuildItem.ItemType.Shield)
        {
            trueShieldValue += buildItem.placingItemUI.statValue;
            DOTween.To(() => shieldValueView, x => shieldValueView = (int)x, trueShieldValue, 1);
        }
    }

    public void HandleStatsDecrease(BuildItem buildItem)
    {
        if (buildItem.itemType == BuildItem.ItemType.Fuel)
        {
            trueFuelValue -= buildItem.placingItemUI.statValue;
            DOTween.To(() => fuelValueView, x => fuelValueView = (int)x, trueFuelValue, 1);
        }
        if (buildItem.itemType == BuildItem.ItemType.Shield)
        {
            trueShieldValue -= buildItem.placingItemUI.statValue;
            DOTween.To(() => shieldValueView, x => shieldValueView = (int)x, trueShieldValue, 1);
        }
    }

    public void SetZeroStats()
    {
        trueFuelValue = 0;
        trueShieldValue = 0;

        DOTween.To(() => fuelValueView, x => fuelValueView = (int)x, trueFuelValue, 1);
        DOTween.To(() => shieldValueView, x => shieldValueView = (int)x, trueShieldValue, 1);
    }
}