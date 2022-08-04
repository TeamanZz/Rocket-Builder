using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class RocketPartPopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public Image rocketPartBackground;
    public Image rocketPartIcon;

    private IEnumerator IEShowText()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        popupText.DOFade(1, 0.5f).startValue = new Color(1, 1, 1, 0);
        rocketPartBackground.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        rocketPartBackground.transform.DORotate(new Vector3(0, 360, 0), 1f,RotateMode.FastBeyond360);
        yield return new WaitForSeconds(2f);
        rocketPartBackground.transform.DORotate(new Vector3(0, 360, 0), 1f,RotateMode.FastBeyond360);
        transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
        popupText.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    
    private void OnEnable()
    {
        StartCoroutine(IEShowText());
    }
}