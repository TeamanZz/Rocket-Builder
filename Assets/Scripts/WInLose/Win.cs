using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private GameObject[] thingsToSetFalse;
    [SerializeField] private GameObject secondCamera;
    [SerializeField] private GameObject blackScreenPrefab, winScreenPrefab;
    [SerializeField] private Transform startPos, finalPos;
    [SerializeField] private LevelProgress levelProgress;
    [SerializeField] private GameObject buildings;

    [SerializeField] private GameObject rotatedCamera;

    private void Start()
    {
        levelProgress = FindObjectOfType<LevelProgress>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartScene();
        }
    }

    [ContextMenu("Win")]
    public void StartScene()
    {
        if (PlayerRocket.Instance.shieldParticle.activeSelf)
        {
            PlayerRocket.Instance.shieldParticle.SetActive(false);
        }
        levelProgress.CancelFillTween();
        PlayerRocket.Instance.isPlayerOnPlanet = true;
        for (int i = 0; i < thingsToSetFalse.Length; i++)
        {
            thingsToSetFalse[i].SetActive(false);
        }

        gameObject.GetComponent<BoxCollider>().enabled = false;
        secondCamera.SetActive(true);
        StartCoroutine(FinalScene());
    }

    public IEnumerator FinalScene()
    {
        PlayerRocket.Instance.GetComponent<SpaceShipMovement>().constantVelocity = 0;
        PlayerRocket.Instance.transform.position = startPos.position;
        Debug.Log("PlayerShipOnStartPlace");
        var screen = Instantiate(blackScreenPrefab);
        yield return new WaitForSeconds(2f);
        rotatedCamera.transform.DORotate(new Vector3(0, 0, 0), 10f);
        Destroy(screen);
        buildings.SetActive(true);
        PlayerRocket.Instance.transform.DOMove(finalPos.position, 3f).SetEase(Ease.Linear);
        Debug.Log("PlayerShipOnFinalPlace");
        yield return new WaitForSeconds(3f);
        Instantiate(winScreenPrefab);
    }
}
