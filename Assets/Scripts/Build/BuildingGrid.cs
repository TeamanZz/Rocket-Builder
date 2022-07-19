
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingGrid : MonoBehaviour
{
    [Header("Grids")]
    [SerializeField] private Vector2Int gridSize;
    public List<BuildItem> placedItems = new List<BuildItem>();
    [SerializeField] public BuildItem[,] grid;
    [field: SerializeField] public BuildItem placingItem { get; set; }
    [field: SerializeField] public BuildItemDataBase placingItemData { get; set; }
    [SerializeField] private Transform rocketObject;

    private float maxItemsWeight;
    private float currentItemsWeight;
    private Camera mainCamera;
    private Vector3 debugPosition;
    public BuildItem startCapsule;

    public List<Vector2> connectorsList = new List<Vector2>();

    private void Awake()
    {
        grid = new BuildItem[gridSize.x, gridSize.y];
        mainCamera = Camera.main;
    }

    private void Start()
    {
        grid[4, 6] = startCapsule;
    }

    public void StartPlacingItem(BuildItem placingItemPrefab)
    {
        if (placingItem != null)
        {
            Destroy(placingItem.gameObject);
        }

        placingItem = Instantiate(placingItemPrefab);
        placingItemData = placingItem.GetComponent<BuildItemDataBase>();
        placingItem.transform.parent = rocketObject;
    }

    private void Update()
    {
        if (placingItem == null)
            return;

        var groundPlane = new Plane(Vector3.forward, Vector3.one);
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPos = ray.GetPoint(position);
            var x = Mathf.RoundToInt(worldPos.x);
            var y = Mathf.RoundToInt(worldPos.y);
            placingItem.transform.position = new Vector3(x, y, 0);

            bool available = true;

            if (IsPlaceTaken(x, y))
                available = false;

            placingItem.HandleColor(available);

            if (Input.GetMouseButtonDown(0))
            {
                if (available)
                    PlaceFlyingItem(x, y);
                else
                    Destroy(placingItem.gameObject);

                ClearPlacingVariables();
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < placingItem.Size.x; x++)
        {
            for (int y = 0; y < placingItem.Size.y; y++)
            {
                //Проверка на выход за границы сетки
                if ((placeX + x) >= gridSize.x || (placeY + y) >= gridSize.y || (placeX + x) < 0 || (placeY + y) < 0)
                    return true;

                //Проверка на то, занята ли ячейка
                if (grid[placeX + x, placeY + y])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void PlaceFlyingItem(int placeX, int placeY)
    {
        for (int x = 0; x < placingItem.Size.x; x++)
        {
            for (int y = 0; y < placingItem.Size.y; y++)
            {
                placingItem.currentPosition = new Vector2Int(placeX + x, placeY + y);
                grid[placeX + x, placeY + y] = placingItem;
            }
        }

        AddConnectorsToList(placeX, placeY);
        placedItems.Add(placingItem);
        placingItem.SetNormalColor();

        CheckOnMainRocketPiece(placeX, placeY);
    }

    private void CheckOnMainRocketPiece(int placeX, int placeY)
    {
        //Идём по всем коннекторам новой части ракеты
        for (int i = 0; i < placingItem.connectors.Count; i++)
        {
            //Если на позиции коннектора есть соседняя часть ракеты
            if (grid[placeX + (int)placingItem.connectors[i].x, placeY + (int)placingItem.connectors[i].y])
            {
                var nearItem = grid[placeX + (int)placingItem.connectors[i].x, placeY + (int)placingItem.connectors[i].y];
                //Идём по всем коннекторам соседней части ракеты
                for (int j = 0; j < nearItem.connectors.Count; j++)
                {
                    //Если у новой и у соседней части ракеты обе стороны стыкуются
                    if (placeX == (nearItem.connectors[j].x + nearItem.currentPosition.x) && placeY == (nearItem.connectors[j].y + nearItem.currentPosition.y))
                    {
                        //Если соседняя часть ракеты является частью ракеты (связана с капсулой)
                        if (nearItem.isMainRocketPiece)
                        {
                            placingItem.isMainRocketPiece = true;
                        }
                    }
                }
            }
        }
        if (!placingItem.isMainRocketPiece)
            placingItem.SetGrayColor();
    }

    private void AddConnectorsToList(int placeX, int placeY)
    {
        for (int i = 0; i < placingItem.connectors.Count; i++)
        {
            if (!connectorsList.Contains(new Vector2(placeX + placingItem.connectors[i].x, placeY + placingItem.connectors[i].y)))
                connectorsList.Add(new Vector2(placeX + placingItem.connectors[i].x, placeY + placingItem.connectors[i].y));
        }
    }

    public void CheckOnDisconnectedParts()
    {
        for (var i = 0; i < placedItems.Count; i++)
        {
            if (!placedItems[i].isMainRocketPiece)
            {
                Destroy(placedItems[i].gameObject);
            }
        }
    }

    public void DeleteAllItems()
    {
        for (int i = 0; i < placedItems.Count; i++)
        {
            Destroy(placedItems[i].gameObject);
        }

        DOTween.To(x => currentItemsWeight = x, Mathf.Round(currentItemsWeight), Mathf.Round(0), 1f);
        placedItems.Clear();
    }

    private void ClearPlacingVariables()
    {
        placingItem = null;
        placingItemData = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(debugPosition, 0.5f);

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Gizmos.DrawWireCube(new Vector3(i, j, 0), new Vector3(1f, 1f, 1f));
            }
        }
    }
}