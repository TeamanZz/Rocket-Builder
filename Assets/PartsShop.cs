using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsShop : MonoBehaviour
{
    public List<GameObject> rocketItemsPrefabs = new List<GameObject>();
    public void BuyRocketItem(int itemIndex)
    {
        Vector2Int? freeGridPlace = BuildingGrid.Instance.GetFreePlace();
        Vector3 newPlace = new Vector3(freeGridPlace.Value.x, freeGridPlace.Value.y, 0);
        var newPart = Instantiate(rocketItemsPrefabs[itemIndex], newPlace, Quaternion.identity, PlayerRocket.Instance.rocketContainer);
        var buildItemComponent = newPart.GetComponent<BuildItem>();
        BuildingGrid.Instance.placingItem = buildItemComponent;
        BuildingGrid.Instance.currentPlacingItemPosition = new Vector2(freeGridPlace.Value.x, freeGridPlace.Value.y);
        // BuildingGrid.Instance.grid[freeGridPlace.Value.x, freeGridPlace.Value.y] = buildItemComponent;
        // BuildingGrid.Instance.placedItems.Add(buildItemComponent);
        BuildingGrid.Instance.HandleDropItem();
    }
}