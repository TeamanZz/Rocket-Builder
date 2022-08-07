using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpeedBoosterSpawner : MonoBehaviour
{
    public float minLocalY;
    public float maxLocalY;
    [Space]
    public float minLocalX;
    public float maxLocalX;
    [Space]
    [SerializeField] private GameObject boosterPrefab;
    public bool wasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (wasTriggered)
            return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            wasTriggered = true;
            StartCoroutine(SpawnBooster());
        }
    }

    public IEnumerator SpawnBooster()
    {
        float localY = Random.Range(minLocalY, maxLocalY);
        float localX = Random.Range(minLocalX, maxLocalX);
        var booster = Instantiate(boosterPrefab, transform.TransformPoint(new Vector3(localX, localY, -0.5f)), Quaternion.identity, CommonContainer.Instance.transform);
        booster.transform.localScale = Vector3.zero;
        booster.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        BoostersManager.Instance.AddBoosterToList(booster);
        yield return new WaitForSeconds(3f);
        wasTriggered = false;
    }
}
