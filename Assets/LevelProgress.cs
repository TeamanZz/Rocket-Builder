using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelProgress : MonoBehaviour
{
    public static LevelProgress Instance;

    public float fillTime = 20;
    public Image fillBar;
    public Image icon;

    [Space]
    public float currentHeight;
    public TextMeshProUGUI currentHeightText;
    public float targetHeight;
    public TextMeshProUGUI targetHeightText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        icon.transform.DOLocalMoveY(265, fillTime).SetEase(Ease.Linear);
        var twen = fillBar.DOFillAmount(1, fillTime).SetEase(Ease.Linear);
        DOTween.To(() => currentHeight, x => currentHeight = x, targetHeight, fillTime).SetEase(Ease.Linear);
    }

    private void FixedUpdate()
    {
        currentHeightText.text = (int)currentHeight + " m";
    }
}