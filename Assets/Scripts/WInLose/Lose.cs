using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevelWithSameRocket()
    {
        Menu.Instance.ActivatePreviewScreen();
        // PlayerRocket.Instance.gameObject.SetActive(true);
        PlayerRocket.Instance.RestartRocket();
        Menu.Instance.ResetAllTriggers();
        Menu.Instance.DestroyAllActiveEnemies();
    }
}