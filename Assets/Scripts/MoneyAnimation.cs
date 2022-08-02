using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MoneyAnimation : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.DOLocalMoveX(transform.localPosition.x - 200, 0.5f).SetEase(Ease.InBack);
            moneyText.DOFade(0, 1f);
        }
    }

    void Start()
    {
        Destroy(gameObject, 1);
        transform.DOLocalMoveX(transform.localPosition.x - 200, 0.5f).SetEase(Ease.InBack);
        moneyText.DOFade(0, 1f);
    }
}