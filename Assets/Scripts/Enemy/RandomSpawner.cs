using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private Vector3 randomizePosition;
    [SerializeField] private int minXPosition, maxXpPosition;
    [SerializeField] private int minEnemiesCount, maxEnemiesCount;

    public Transform enemiesParent;

    public bool wasSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if (wasSpawned)
            return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            wasSpawned = true;
            int randomCount = Random.Range(minEnemiesCount, maxEnemiesCount + 1);
            for (int i = 0; i < randomCount; i++)
            {
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy()
    {
        int posX = Random.Range(minXPosition, maxXpPosition + 1);
        var posY = SpaceShipMovement.Instance.gameObject.transform.position.y;
        int index = Random.Range(0, enemiesPrefabs.Length);
        var newEnemy = Instantiate(enemiesPrefabs[index], new Vector3(transform.position.x + posX, randomizePosition.y + posY, randomizePosition.z), Quaternion.identity);
        newEnemy.transform.parent = enemiesParent;
        newEnemy.transform.localScale = Vector3.zero;
        newEnemy.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
}
