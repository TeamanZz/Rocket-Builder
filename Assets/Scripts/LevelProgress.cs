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

    private Tween Totween;
    private Tween iconTween;
    private Tween fillbarTween;
    [SerializeField] private Vector3 startIconPosition;

    private void Awake()
    {
        Instance = this;
        targetHeightText.text = targetHeight.ToString();
    }

    private void Start()
    {
        startIconPosition = icon.transform.position;
        StartTween();
    }

    public void StartTween()
    {
        iconTween = icon.transform.DOLocalMoveY(265, fillTime).SetEase(Ease.Linear);
        fillbarTween = fillBar.DOFillAmount(1, fillTime).SetEase(Ease.Linear);
        Totween = DOTween.To(() => currentHeight, x => currentHeight = x, targetHeight, fillTime).SetEase(Ease.Linear);
    }
    
    private void FixedUpdate()
    {
        currentHeightText.text = (int)currentHeight + " m";
    }

    public void CancelFillTween()
    {
        Totween.Kill();
        iconTween.Kill();
        fillbarTween.Kill();
    }

    public void RestartFillTween()
    {
        currentHeight = 0;
        fillBar.fillAmount = 0;
        icon.transform.position = startIconPosition;
        StartTween();
    }
}