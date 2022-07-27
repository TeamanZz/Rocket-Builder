using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Lose : MonoBehaviour
{
    public Image backgroundImage;

    private void OnEnable()
    {
        backgroundImage.color = new Color(0, 0, 0, 0);
        backgroundImage.DOFade(1, 2);
        StartCoroutine(RestartLevelAfterDelay());
    }

    private IEnumerator RestartLevelAfterDelay()
    {
        yield return new WaitForSeconds(3);
        RestartLevelWithSameRocket();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevelWithSameRocket()
    {
        Menu.Instance.ActivatePreviewScreen();
        PlayerRocket.Instance.RestartRocket();
        Menu.Instance.ResetAllTriggers();
        Menu.Instance.DestroyAllActiveEnemies();
    }
}