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

    public Image damageIndicator;
    private Coroutine damageIndicatorCoroutine;
    private Tween damageIndicatorTween;

    private void Awake()
    {
        Instance = this;
        targetHeightText.text = targetHeight.ToString();
    }

    public void StartHeightBarFilling()
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

    public void ResetHeightVariables()
    {
        currentHeight = 0;
        fillBar.fillAmount = 0;
        Debug.Log(fillBar.fillAmount + "fill");
        icon.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    [ContextMenu("ShowDamageIndication")]
    public void ShowDamageIndication()
    {
        if (damageIndicatorCoroutine != null)
        {
            DOTween.Kill(damageIndicator);
            StopCoroutine(damageIndicatorCoroutine);
        }
        damageIndicatorCoroutine = StartCoroutine(IEShowDamageIndication());
    }

    private IEnumerator IEShowDamageIndication()
    {
        Debug.Log("damage");
        damageIndicator.color = new Color(1, 1, 1, 0);
        damageIndicator.gameObject.SetActive(true);
        damageIndicatorTween = damageIndicator.DOFade(1, 0.2f).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(0.4f);
        damageIndicator.gameObject.SetActive(false);
    }
}