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

    [Header("HighSCoreLine")]
    [SerializeField] private GameObject highSCoreLinePrefab;

    public GameObject currentHighScoreLine;

    private void Awake()
    {
        Instance = this;
        // targetHeightText.text = targetHeight.ToString();
    }

    public void StartHeightBarFilling()
    {
        var speed = SpaceShipMovement.Instance.GetTrueSpeed();
        var distance = LevelsHandler.Instance.GetTargetPlanetPosition();
        fillTime = (distance - SpaceShipMovement.Instance.transform.position.y) / speed;
        fillTime -= (fillTime * 0.2f);
        iconTween = icon.transform.DOLocalMoveY(265, fillTime).SetEase(Ease.Linear);
        fillbarTween = fillBar.DOFillAmount(1, fillTime).SetEase(Ease.Linear);
        var newTargetHeight = LevelsHandler.Instance.GetTargetDistanceNuber();
        Totween = DOTween.To(() => currentHeight, x => currentHeight = x, newTargetHeight, fillTime).SetEase(Ease.Linear);
        targetHeightText.text = newTargetHeight + " km";
    }

    private void FixedUpdate()
    {
        currentHeightText.text = (int)currentHeight + " km";
    }

    public void CancelFillTween()
    {
        Totween.Kill();
        iconTween.Kill();
        fillbarTween.Kill();
    }

    public void StopFillingHeightBar()
    {
        Totween.Pause();
        iconTween.Pause();
        fillbarTween.Pause();
    }

    public void ContinueFillingHeightBar()
    {
        Totween.Play();
        iconTween.Play();
        fillbarTween.Play();
    }

    public void ResetHeightVariables()
    {
        currentHeight = 0;
        fillBar.fillAmount = 0;
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
        damageIndicator.color = new Color(1, 1, 1, 0);
        damageIndicator.gameObject.SetActive(true);
        damageIndicatorTween = damageIndicator.DOFade(1, 0.2f).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(0.4f);
        damageIndicator.gameObject.SetActive(false);
    }

    public void SetHighSCoreLine()
    {
        if (currentHighScoreLine != null)
        {
            Destroy(currentHighScoreLine.gameObject);
            var HeightLine = Instantiate(highSCoreLinePrefab, new Vector3(0, PlayerRocket.Instance.transform.position.y, 8), Quaternion.identity, CommonContainer.Instance.transform);
            HeightLine.GetComponent<HeighScoreLine>().heightScoreText.text = Mathf.RoundToInt(currentHeight).ToString() + "km";
            currentHighScoreLine = HeightLine;
        }
        else
        {
            var HeightLine = Instantiate(highSCoreLinePrefab, new Vector3(0, PlayerRocket.Instance.transform.position.y, 8), Quaternion.identity, CommonContainer.Instance.transform);
            HeightLine.GetComponent<HeighScoreLine>().heightScoreText.text = Mathf.RoundToInt(currentHeight).ToString() + "km";
            currentHighScoreLine = HeightLine;
        }
    }

    public void DeleteHighScoreLineAtWin()
    {
        Destroy(currentHighScoreLine.gameObject);
        currentHighScoreLine = null;
    }
}