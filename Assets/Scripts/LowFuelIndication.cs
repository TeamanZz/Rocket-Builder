using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LowFuelIndication : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private void OnEnable()
    {
        canvasGroup.DOFade(1, 0.8f).SetLoops(-1, LoopType.Yoyo);
    }
}