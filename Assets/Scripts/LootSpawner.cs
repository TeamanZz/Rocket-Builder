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
    public bool wasSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if (wasSpawned)
            return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            wasSpawned = true;
            if (currentDuplicatesValue < maxDuplicatesAllowed)
                SpawnLootMeteor();
        }
    }

    public void SpawnLootMeteor()
    {
        currentDuplicatesValue++;
        float localY = Random.Range(minLocalY, maxLocalY);
        float localX = Random.Range(minLocalX, maxLocalX);

        var lootMeteor = Instantiate(lootPrefab, transform.TransformPoint(new Vector3(localX, localY, -0.5f)), Quaternion.identity, lootContainer);
    }
}