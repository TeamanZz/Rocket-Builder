using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    public float minLocalY;
    public float maxLocalY;

    public float minLocalX;
    public float maxLocalX;

    public GameObject lootPrefab;
    public Transform lootContainer;

    private void Start()
    {
        SpawnLootMeteor();
    }
    public void SpawnLootMeteor()
    {
        float localY = Random.Range(minLocalY, maxLocalY);
        float localX = Random.Range(minLocalX, maxLocalX);

        var lootMeteor = Instantiate(lootPrefab, transform.TransformPoint(new Vector3(localX, localY, -0.5f)), Quaternion.identity, lootContainer);
    }
}