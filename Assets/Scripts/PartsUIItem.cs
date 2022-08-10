using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartsUIItem : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI statText;
    public TextMeshProUGUI priceText;
    public GameObject statIcon;
    public Image mainImage;
    public float statValue;
    public int price;
    public int countValue;
    public Button buyButton;
    public int index;

    public int totalCountValue;

    public bool isBoughted;
    [field: SerializeField] public string partName { get; private set; }

    private void Awake()
    {
        statText.text = statValue.ToString();
        totalCountValue = PlayerPrefs.GetInt($"{index} Item Count", countValue);
        HandleView();
    }

    public void DescreaseCount()
    {
        countValue--;
        HandleView();
    }

    public void IncreaseCount()
    {
        countValue++;
        HandleView();
    }

    public void BuyItem()
    {
        if (!Money.Instance.IsEnoughCurrency(price))
            return;
        Money.Instance.DecreaseCurrency(price);
        isBoughted = true;
        countText.gameObject.SetActive(true);
        statText.gameObject.SetActive(true);
        statIcon.gameObject.SetActive(true);
        HandleView();
        PlayerPrefs.SetInt($"{index} Item IsBoughted", 1);
        NewPartsHandler.Instance.boughtedItems.Add(this);
        buyButton.gameObject.SetActive(false);
    }

    public void HandleView()
    {
        if (!isBoughted)
        {
            countText.gameObject.SetActive(false);
            statText.gameObject.SetActive(false);
            statIcon.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            priceText.text = "Buy: " + price.ToString() + " $";
            mainImage.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        }
        else
        {
            countText.text = "x" + countValue.ToString();
            if (countValue == 0)
                mainImage.color = new Color(1f, 1f, 1f, 0.5f);
            else
                mainImage.color = Color.white;
        }

    }
}