using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinPanel : MonoBehaviour
{
    public Image backgroundBlack;
    public GameObject continueButton;

    public void PlayFade()
    {
        StartCoroutine(IEPlayFade());
    }

    private IEnumerator IEPlayFade()
    {
        backgroundBlack.gameObject.SetActive(true);
        FadeIn();
        yield return new WaitForSeconds(1);
        FadeOut();
        yield return new WaitForSeconds(1);
        continueButton.SetActive(true);
    }

    public void FadeIn()
    {
        backgroundBlack.color = new Color(0, 0, 0, 0);
        backgroundBlack.DOFade(1, 1f);
    }

    public void FadeOut()
    {
        backgroundBlack.color = new Color(0, 0, 0, 1);
        backgroundBlack.DOFade(0, 1f);
    }
}