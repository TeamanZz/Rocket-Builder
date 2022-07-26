using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class RocketPartPopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;

    private IEnumerator IEShowText()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        popupText.DOFade(1, 0.5f).startValue = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(2f);
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