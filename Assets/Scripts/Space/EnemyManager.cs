using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public List<EnemyBase> enemiesContainer = new List<EnemyBase>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddEnemyAtEnemiesContainer(EnemyBase enemy)
    {
        enemiesContainer.Add(enemy);
    }

    public void RemoveEnemyFromContainer(EnemyBase enemy)
    {
        enemiesContainer.Remove(enemy);
    }

    public void ClearEnemyList()
    {
        enemiesContainer.Clear();
    }
}
