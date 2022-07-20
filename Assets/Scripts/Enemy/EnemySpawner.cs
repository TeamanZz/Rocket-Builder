using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;

    private void Start()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int countOfEnemies;
            Random random = new Random();
            countOfEnemies = random.Next(1, enemies.Length + 1);
            for (int i = 0; i < countOfEnemies; i++)
            {
                enemies[i].SetActive(true);
            }
            Debug.Log(countOfEnemies);
        }
    }
}
