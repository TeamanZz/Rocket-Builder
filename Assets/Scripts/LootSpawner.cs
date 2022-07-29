using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    public float minLocalY;
    public float maxLocalY;
    [Space]
    public float minLocalX;
    public float maxLocalX;
    [Space]
    public int maxDuplicatesAllowed = 5;
    public int currentDuplicatesValue = 0;
    [Space]
    public GameObject lootPrefab;
    public Transform lootContainer;

    private void Start()
    {
        // var randValue = Random.Range(0, 101);
        // if (randValue <= spawnChance)
        if (currentDuplicatesValue < maxDuplicatesAllowed)
            SpawnLootMeteor();
    }
    public void SpawnLootMeteor()
    {
        currentDuplicatesValue++;
        float localY = Random.Range(minLocalY, maxLocalY);
        float localX = Random.Range(minLocalX, maxLocalX);

        var lootMeteor = Instantiate(lootPrefab, transform.TransformPoint(new Vector3(localX, localY, -0.5f)), Quaternion.identity, lootContainer);
    }
}