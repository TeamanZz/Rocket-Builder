
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingGrid : MonoBehaviour
{
    public static BuildingGrid Instance;
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

    private Vector2 currentPlacingItemPosition;

    private void Awake()
    {
        Instance = this;
        grid = new BuildItem[gridSize.x, gridSize.y];
        mainCamera = Camera.main;
    }

    private void Start()
    {
        grid[4, 6] = startCapsule;
    }

    public void ToggleItemsConnectors()
    {
        for (var i = 0; i < placedItems.Count; i++)
        {
            placedItems[i].ToggleConnectors();
        }
    }

    public void StartPlacingItem(BuildItem placingItemPrefab)
    {
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
            float x;
            float y;
            Vector3 worldPos = ray.GetPoint(position);
            if ((worldPos.x) >= gridSize.x - 0.5f || (worldPos.y) >= gridSize.y - 0.5f || (worldPos.x) <= -0.5f || (worldPos.y) <= -0.5f)
            {
                x = worldPos.x;
                y = worldPos.y;
            }
            else
            {
                x = Mathf.RoundToInt(worldPos.x);
                y = Mathf.RoundToInt(worldPos.y);
            }
            currentPlacingItemPosition = new Vector2(x, y);
            placingItem.transform.position = new Vector3(x, y, 0);
        }
    }

    public void HandleDropItem()
    {
        bool available = true;

        if (IsPlaceTaken(Mathf.RoundToInt(currentPlacingItemPosition.x), Mathf.RoundToInt(currentPlacingItemPosition.y)))
            available = false;

        if (available)
            PlaceFlyingItem();
        else
            Destroy(placingItem.gameObject);

        ClearPlacingVariables();
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

    public void PlaceFlyingItem()
    {
        int placeX = Mathf.RoundToInt(currentPlacingItemPosition.x);
        int placeY = Mathf.RoundToInt(currentPlacingItemPosition.y);

        for (int x = 0; x < placingItem.Size.x; x++)
        {
            for (int y = 0; y < placingItem.Size.y; y++)
            {
                placingItem.placedPosition = new Vector2Int(placeX + x, placeY + y);

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
            //Проверка на выход за границы сетки
            if ((placeX + placingItem.connectors[i].x) >= gridSize.x || (placeY + placingItem.connectors[i].y) >= gridSize.y || (placeX + placingItem.connectors[i].x) < 0 || (placeY + placingItem.connectors[i].y) < 0)
                continue;

            //Если на позиции коннектора есть соседняя часть ракеты
            if ((grid[placeX + (int)placingItem.connectors[i].x, placeY + (int)placingItem.connectors[i].y]))
            {
                var nearItem = grid[placeX + (int)placingItem.connectors[i].x, placeY + (int)placingItem.connectors[i].y];
                //Идём по всем коннекторам соседней части ракеты
                for (int j = 0; j < nearItem.connectors.Count; j++)
                {
                    //Если у новой и у соседней части ракеты обе стороны стыкуются
                    if (placeX == (nearItem.connectors[j].x + nearItem.placedPosition.x) && placeY == (nearItem.connectors[j].y + nearItem.placedPosition.y))
                    {
                        //Если соседняя часть ракеты является частью ракеты (связана с капсулой)
                        if (nearItem.isMainRocketPiece)
                        {
                            placingItem.isMainRocketPiece = true;

                            for (int z = 0; z < placingItem.connectors.Count; z++)
                            {
                                CheckOnNewRocketPieces(placingItem);
                            }
                        }
                    }
                }
            }
        }
        if (!placingItem.isMainRocketPiece)
            placingItem.SetGrayColor();
    }

    public void CheckOnNewRocketPieces(BuildItem itemToCheck)
    {
        //Идём по всем коннекторам новой части ракеты
        for (int i = 0; i < itemToCheck.connectors.Count; i++)
        {

            //Проверка на выход за границы сетки
            if ((itemToCheck.placedPosition.x + placingItem.connectors[i].x) >= gridSize.x ||
            (itemToCheck.placedPosition.y + placingItem.connectors[i].y) >= gridSize.y ||
            (itemToCheck.placedPosition.x + placingItem.connectors[i].x) < 0 ||
            (itemToCheck.placedPosition.y + placingItem.connectors[i].y) < 0)
                continue;

            //Если на позиции коннектора есть соседняя часть ракеты
            if (grid[itemToCheck.placedPosition.x + (int)itemToCheck.connectors[i].x, itemToCheck.placedPosition.y + (int)itemToCheck.connectors[i].y])
            {
                var nearItem = grid[itemToCheck.placedPosition.x + (int)itemToCheck.connectors[i].x, itemToCheck.placedPosition.y + (int)itemToCheck.connectors[i].y];
                //Идём по всем коннекторам соседней части ракеты
                for (int j = 0; j < nearItem.connectors.Count; j++)
                {
                    //Если у новой и у соседней части ракеты обе стороны стыкуются
                    if (itemToCheck.placedPosition.x == (nearItem.connectors[j].x + nearItem.placedPosition.x) && itemToCheck.placedPosition.y == (nearItem.connectors[j].y + nearItem.placedPosition.y))
                    {
                        //Если соседняя часть ракеты является частью ракеты (связана с капсулой)
                        if (!nearItem.isMainRocketPiece)
                        {
                            nearItem.isMainRocketPiece = true;
                            nearItem.SetNormalColor();
                            for (int z = 0; z < itemToCheck.connectors.Count; z++)
                            {
                                CheckOnNewRocketPieces(nearItem);
                            }
                        }
                    }
                }
            }
        }
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