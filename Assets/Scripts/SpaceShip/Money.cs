using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public static Money Instance;

    [SerializeField] private int currencyValue;
    [SerializeField] private TextMeshProUGUI currencyText;
    public GameObject moneyTextPrefab;
    public Transform moneyTextSpawnPos;
    public Transform moneyTextParent;

    private void Awake()
    {
        Instance = this;
        HandleView();
    }

    public void HandleView()
    {
        currencyText.text = currencyValue.ToString() + " $";
    }

    public bool IsEnoughCurrency(int value)
    {
        if (currencyValue >= value)
            return true;
        else
            return false;
    }

    public void AddCurrency(int count)
    {
        var moneyWithMultiple = (count * (LevelsHandler.Instance.currentLevelIndex + 1));
        currencyValue += moneyWithMultiple;
        HandleView();
        var moneyText = Instantiate(moneyTextPrefab, moneyTextSpawnPos.position, Quaternion.identity, moneyTextParent);
        moneyText.GetComponent<TextMeshProUGUI>().text = moneyWithMultiple.ToString() + "$";
    }

    public void DecreaseCurrency(int count)
    {
        currencyValue -= count;
        HandleView();
    }
}