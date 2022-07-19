using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildItem : MonoBehaviour
{
    public int id;
    [Header("ItemSettings")]
    public Vector2Int Size = Vector2Int.one;
    public List<Vector2> AllowedToBuildCells = new List<Vector2>();

    [SerializeField] private Renderer mainRenderer;
    [SerializeField] private BuildingGrid buildingGrid;

    [Header("Connectors")]
    [SerializeField] public bool isThisItemHasConnectors;
    [SerializeField] public List<BuildItem> connectorList = new List<BuildItem>();

    private void Awake()
    {
        buildingGrid = FindObjectOfType<BuildingGrid>();
        mainRenderer = GetComponentInChildren<Renderer>();
    }

    public void SetTransparent(bool available)
    {
        if (available)
            mainRenderer.material.color = new Color(0f, 1f, 0.07f, 0.55f);
        else
            mainRenderer.material.color = new Color(1f, 0f, 0.02f, 0.62f);
    }

    public void SetNormal()
    {
        mainRenderer.material.color = Color.white;
    }

    public void DeleteItemFromGrid()
    {
        if (id <= 0)
            id = 1;

        buildingGrid.placingItem = gameObject.GetComponent<BuildItem>();
        buildingGrid.RemoveWeightFromText();
        buildingGrid.placedItems.RemoveAt(id - 1);
        buildingGrid.countOfItems--;

        for (int i = 0; i < buildingGrid.placedItems.Count; i++)
        {
            buildingGrid.placedItems[i].id -= 1;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Gizmos.color = new Color(0f, 0.12f, 1f, 0.56f);
                Gizmos.DrawCube(transform.position + new Vector3(x, y, 0), new Vector3(1f, 1f, 1f));
            }
        }

        for (int i = 0; i < AllowedToBuildCells.Count; i++)
        {
            Gizmos.color = new Color(0f, 1f, 0.17f, 0.56f);
            Gizmos.DrawCube(transform.position + new Vector3(AllowedToBuildCells[i].x, AllowedToBuildCells[i].y, 0), new Vector3(1f, 1f, 1f));
        }

    }
}