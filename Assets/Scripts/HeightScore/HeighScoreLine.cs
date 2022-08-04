using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HeighScoreLine : MonoBehaviour
{
    public TextMeshProUGUI heightScoreText;

    private void Start()
    {
        transform.DOScale(1, 0.5f);
    }
}
