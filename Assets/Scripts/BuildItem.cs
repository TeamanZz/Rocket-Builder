using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildItem : MonoBehaviour
{
    [Header("ItemSettings")]
    [SerializeField] public Vector2Int Size = Vector2Int.one;
    [SerializeField] private Renderer mainRenderer;
    public int id;
    [SerializeField] private BuildingGrid buildingGrid;
    [field: SerializeField] public float itemWeight { get; private set; }
    

    private void Awake()
    {
        buildingGrid = FindObjectOfType<BuildingGrid>();
        mainRenderer = GetComponentInChildren<Renderer>();
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Gizmos.color = new Color(0f, 0.12f, 1f, 0.56f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1f, 1f, 1f));
            }
        }
    }

    public void SetTransparent(bool available)
    {
        if (available)
        {
            mainRenderer.material.color = new Color(0f, 1f, 0.07f, 0.55f);
        }
        else
        {
            mainRenderer.material.color = new Color(1f, 0f, 0.02f, 0.62f);
        }
    }

    public void SetNormal()
    {
        mainRenderer.material.color = Color.white;
    }


    public void DeleteItemFromGrid()
    {
        if (id <= 0)
        {
            id = 1;
        }
        buildingGrid.placingItem = gameObject.GetComponent<BuildItem>();
        buildingGrid.RemoveWeightFromText();
        buildingGrid.placedItems.RemoveAt(id - 1);
        buildingGrid.countOfItems--;
        Destroy(gameObject);
        for (int i = 0; i < buildingGrid.placedItems.Count; i++)
        {
            buildingGrid.placedItems[i].id -= 1;
        }
    }

    private void OnMouseDown()
    {
        DeleteItemFromGrid();
    }
}