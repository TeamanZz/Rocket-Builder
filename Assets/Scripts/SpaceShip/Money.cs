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
        currencyValue += count;
        HandleView();
    }

    public void DecreaseCurrency(int count)
    {
        currencyValue -= count;
        HandleView();
    }
}