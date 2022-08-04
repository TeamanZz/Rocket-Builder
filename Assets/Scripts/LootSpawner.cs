using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public GameObject[] lootPrefabs;
    public Transform lootContainer;
    public bool wasSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if (wasSpawned)
            return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            wasSpawned = true;
            // if (currentDuplicatesValue < maxDuplicatesAllowed)
            SpawnLootMeteor();
        }
    }

    public void SpawnLootMeteor()
    {
        float localY = Random.Range(minLocalY, maxLocalY);
        float localX = Random.Range(minLocalX, maxLocalX);
        var randomNumber = Random.Range(1, lootPrefabs.Length);
        var lootMeteor = Instantiate(lootPrefabs[randomNumber], transform.TransformPoint(new Vector3(localX, localY, -0.5f)), Quaternion.identity, lootContainer);
        Debug.Log(randomNumber + "Container =" + lootPrefabs[randomNumber].name);
    }
}