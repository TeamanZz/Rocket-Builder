using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private Vector3 randomizePosition;
    [SerializeField] private int minXPosition, maxXpPosition;
    [SerializeField] private int minEnemiesCount, maxEnemiesCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Random random = new Random();
            int randomCount = random.Next(minEnemiesCount,maxEnemiesCount);
            for (int i = 0; i < randomCount; i++)
            {
                SpawnEnemy();
            }
        }
    }

    
    public void SpawnEnemy()
    {
        Random random = new Random();
        int posX = random.Next(minXPosition, maxXpPosition);
        var posY = SpaceShipMovement.Instance.gameObject.transform.position.y;
        Random randomIndex = new Random();
        int index = randomIndex.Next(1, enemiesPrefabs.Length);
        Instantiate(enemiesPrefabs[index], new Vector3(posX, randomizePosition.y + posY, randomizePosition.z),
                    Quaternion.identity);
    }
}
