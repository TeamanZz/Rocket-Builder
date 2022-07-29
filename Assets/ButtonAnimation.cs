using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    private Vector3 defaultScale;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void PlayClickAnimation()
    {
        transform.DOScale(defaultScale - Vector3.one * 0.08f, 0.1f).SetLoops(2, LoopType.Yoyo).From(defaultScale).SetEase(Ease.Linear);
    }
}
