
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [Header("Grids")] 
    [SerializeField] private Vector2Int gridSize;
    public List<BuildItem> placedItems = new List<BuildItem>();
    public int countOfItems;
    [SerializeField] public BuildItem[,] grid;
    public List<BuildItem[,]> gridList = new List<BuildItem[,]>();
    [SerializeField] private Transform normalBuildPosition;
    [SerializeField] private Vector3 debugPosition;
    [field: SerializeField] public BuildItem placingItem { get; set; }
    [SerializeField] private Transform rocketObject;
    [SerializeField] private List<BuildItem> connectors = new List<BuildItem>();

    [Header("ControlText")] [SerializeField]
    private float maxItemsWeight;
    [SerializeField] private float currentItemsWeight;
    [Space(5f)] [Header("UI")] [SerializeField]
    private Text weightControlText;

    
    
    [Header("EndBuild")] 
    [SerializeField] private Transform startRocketPosition;
    [SerializeField] private Camera rocketCamera;
    [SerializeField] private GameObject blackScreenPrefab;
    [SerializeField] private GameObject uiItems;

    private void Awake()
    {
        weightControlText.text = $"Weight : {currentItemsWeight} / {maxItemsWeight}";
        grid = new BuildItem[gridSize.x, gridSize.y];
        mainCamera = Camera.main;
    }

    public void ResizeGrid()
    {
        var xScale = gridSize.x / 10;
        var yScale = gridSize.y / 10;
        transform.localScale = new Vector3(xScale,1,yScale);
    }

    public void StartPlacingItem(BuildItem placingItemPrefab)
    {
        if (placingItem != null)
        {
            Destroy(placingItem.gameObject);
            for (int i = 0; i < placingItem.connectorList.Count; i++)
            {
                connectors.RemoveAt(i);
                Destroy(connectors[i]);
                i--;
            }
        }

        placingItem = Instantiate(placingItemPrefab);
        placingItem.transform.parent = rocketObject;
        for (int i = 0; i < placingItem.connectorList.Count; i++)
        {
            connectors.Add(placingItem.connectorList[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(debugPosition,0.5f);

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Gizmos.DrawWireCube(new Vector3(i,j,0), new Vector3(1f,1f,1f));
            }
        }
    }

    private void Update()
    {

        weightControlText.text = (currentItemsWeight).ToString("0.0") + "/" + (maxItemsWeight).ToString("0.0");

        if (placingItem == null) 
            return;

        var groundPlane = new Plane(Vector3.forward, Vector3.one);
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        
        
        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPos = ray.GetPoint(position);
            debugPosition = worldPos;
            //Debug.Log(worldPos);
            var x = Mathf.RoundToInt(worldPos.x);
            var y = Mathf.RoundToInt(worldPos.y);

            bool available = true;

            if (x < 0 || x > gridSize.x - placingItem.Size.x)
            {
                available = false;
            }

            if (y < 0 || y > gridSize.y - placingItem.Size.y)
            {
                available = false;
            }

            if (available && IsPlaceTaken(x, y))
            {
                available = false;
            }

            placingItem.transform.position = new Vector3(x, y, 0);
            
            placingItem.SetTransparent(available);

            if (Input.GetMouseButtonDown(0) && available == true &&
                currentItemsWeight + placingItem.itemWeight < maxItemsWeight)
            {
                PlaceFlyingItem(x, y);
            }
            else if (Input.GetMouseButtonDown(0) && available == false)
            {
                Destroy(placingItem.gameObject);
                placingItem = null;
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        if (placingItem.isThisItemHasConnectors)
        {
            return false;
        }
        else
        {
            
            
            for (int x = 0; x < placingItem.Size.x; x++)
            {
                for (int y = 0; y < placingItem.Size.y; y++)
                {
                    
                    if (grid[placeX + x, placeY + y] != null)
                    {
                        return true;
                    }
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
                grid[placeX + x, placeY + y] = placingItem;
            }
        }

        for (int i = 0; i < placingItem.AllowedToBuildCells.Count; i++)
        {
            Debug.Log(i + "" + " point = " + " " + placingItem.AllowedToBuildCells[i].x);
        }
        Debug.Log(placingItem.transform.position);
        
        AddWeightToText();
        placedItems.Add(placingItem);   
        placingItem.SetNormal();
        countOfItems++;
        placingItem.id = countOfItems;
        placingItem = null;
    }

    public void AddWeightToText()
    {
        DOTween.To(x => currentItemsWeight = x, Mathf.Round(currentItemsWeight),
            Mathf.Round(currentItemsWeight + placingItem.itemWeight), 1f);
    }

    public void RemoveWeightFromText()
    {
        DOTween.To(x => currentItemsWeight = x, Mathf.Round(currentItemsWeight),
            Mathf.Round(currentItemsWeight - placingItem.itemWeight), 1f);
    }

    public void DeclineItem()
    {
        if (placingItem != null)
        {
            placingItem = null;
        }
        else
        {
            Destroy(placingItem.gameObject);
            placingItem = null;
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
        countOfItems = 0;
        connectors.Clear();
    }

    public void EndBuidling()
    {
        StartCoroutine(EndBuildingCorutine());
    }
    
    public IEnumerator EndBuildingCorutine()
    {
        var blackScreen = Instantiate(blackScreenPrefab);
        uiItems.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(blackScreen);
        rocketObject.transform.position = startRocketPosition.position;
        rocketCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }
}