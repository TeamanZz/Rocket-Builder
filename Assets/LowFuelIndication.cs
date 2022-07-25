using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LowFuelIndication : MonoBehaviour
{
    public TextMeshProUGUI lowFuelText;
    private void OnEnable()
    {
        lowFuelText.DOFade(1, 1).SetLoops(-1, LoopType.Yoyo);
    }
}