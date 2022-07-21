using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject[] itemsToActive;
    [SerializeField] private GameObject[] itemsToUnActive;
    [SerializeField] private GameObject blackScreenPrefab;

    public void ClickOnStartGame()
    {
        StartCoroutine(StartGameScreenSetToActive());
    }

    public IEnumerator StartGameScreenSetToActive()
    {
        var screen = Instantiate(blackScreenPrefab);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < itemsToActive.Length; i++)
        {
            itemsToActive[i].SetActive(true);
        }

        for (int i = 0; i < itemsToUnActive.Length; i++)
        {
            itemsToUnActive[i].SetActive(false);
        }
        Destroy(screen,2f);
    }
}
