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
    [SerializeField] public List<Vector2> AllowedToBuildCells = new List<Vector2>();
    [SerializeField] private Renderer mainRenderer;
    public int id;
    [SerializeField] private BuildingGrid buildingGrid;
    [field: SerializeField] public float itemWeight { get; private set; }
    
    [Header("Connectors")]
    [SerializeField] public List<BuildItem> connectorList = new List<BuildItem>();

    [SerializeField] public bool isThisItemHasConnectors;

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
                Gizmos.DrawCube(transform.position + new Vector3(x, y, 0), new Vector3(1f, 1f, 1f));
            }
        }

        for (int i = 0; i < AllowedToBuildCells.Count; i++)
        {
            Gizmos.color = new Color(0f, 1f, 0.17f, 0.56f);
            Gizmos.DrawCube(transform.position + new Vector3(AllowedToBuildCells[i].x, AllowedToBuildCells[i].y, 0), new Vector3(1f, 1f, 1f));
            /*for (int x = 0; x <= AllowedToBuildCells[i].x; x++)
            {
                for (int y = 0; y <= AllowedToBuildCells[i].y; y++)
                {
                    
                }
            }*/
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
        //DeleteItemFromGrid();
    }

    /*public void SetCellsPositions()
    {
        var xPos = Convert.ToInt32(gameObject.transform.position.x);
        var yPos = Convert.ToInt32(gameObject.transform.position.y);

        if (AllowedToBuildCells.Count == 4)
        {
            var xPos0 = AllowedToBuildCells[0].x;
            var xPos1 = AllowedToBuildCells[1].x;
            var xPos2 = AllowedToBuildCells[2].x;
            var xPos3 = AllowedToBuildCells[3].x;
            
            var yPos0 = AllowedToBuildCells[0].y;
            var yPos1 = AllowedToBuildCells[1].y;
            var yPos2 = AllowedToBuildCells[2].y;
            var yPos3 = AllowedToBuildCells[3].y;
            
            xPos0 = xPos;
            xPos1 = xPos;
            xPos2 = xPos - 1;
            xPos3 = xPos + 1;

            yPos0 = yPos0 - 1;
            yPos1 = yPos + 1;
            yPos2 = yPos;
            yPos3 = yPos;

            AllowedToBuildCells[0] = new Vector2(xPos0,yPos0);
            AllowedToBuildCells[1] = new Vector2(xPos1,yPos1);
            AllowedToBuildCells[2] = new Vector2(xPos2,yPos2);
            AllowedToBuildCells[3] = new Vector2(xPos3,yPos3);
        }
    }*/
}