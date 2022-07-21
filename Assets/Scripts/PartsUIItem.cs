using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartsUIItem : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI statText;
    public Image mainImage;
    public int statValue;
    public int countValue;

    private void Awake()
    {
        statText.text = statValue.ToString();
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

    public void HandleView()
    {
        countText.text = "x" + countValue.ToString();
        if (countValue == 0)
        {
            mainImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }
        else
        {
            mainImage.color = Color.white;
        }
    }
}