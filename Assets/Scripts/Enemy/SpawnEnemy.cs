using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Vector3 spawnCoordinate;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private SpaceShipMovement playerShip;

    private void Start()
    {
        playerShip = FindObjectOfType<SpaceShipMovement>();
        spawnPosition = playerShip.spawnEnemyPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SpaceShipMovement>())
        {
            var enemy = Instantiate(enemyPrefab, new Vector3(-100, 100, 7), Quaternion.identity);
            enemy.transform.DOMove(spawnPosition.position + spawnCoordinate, 3f).SetEase(Ease.Linear);
        }
    }
}