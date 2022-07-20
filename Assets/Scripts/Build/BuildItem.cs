using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildItem : MonoBehaviour
{
    public int id;
    [SerializeField] private Renderer mainRenderer;

    [Header("ItemSettings")]
    public Vector2Int Size = Vector2Int.one;
    public Vector2Int placedPosition = Vector2Int.one;
    public List<Vector2> connectors = new List<Vector2>();
    public List<Transform> connectorsGO = new List<Transform>();

    private BuildingGrid buildingGrid;

    public bool isMainRocketPiece;
    public bool isMainCapsule;

    private void Awake()
    {
        buildingGrid = FindObjectOfType<BuildingGrid>();
        // mainRenderer = GetComponentInChildren<Renderer>();
    }

    public void IncreaseScale()
    {
        transform.localScale = Vector3.one * 0.55f;
    }

    public void SetNormalScale()
    {
        transform.localScale = Vector3.one * 0.5f;
    }

    public void ToggleConnectors()
    {
        for (var i = 0; i < connectorsGO.Count; i++)
        {
            connectorsGO[i].gameObject.SetActive(!connectorsGO[i].gameObject.activeSelf);
        }
    }

    public void HandleColorGreenOrRed(bool available)
    {
        if (available)
            mainRenderer.material.color = new Color(0f, 1f, 0.07f, 0.55f);
        else
            mainRenderer.material.color = new Color(1f, 0f, 0.02f, 0.62f);
    }

    public void HandleColorWhiteOrGray()
    {
        if (isMainRocketPiece)
            SetNormalColor();
        else
            SetGrayColor();
    }

    public void SetNormalColor()
    {
        mainRenderer.material.color = Color.white;
    }

    public void SetGrayColor()
    {
        mainRenderer.material.color = new Color(0.02f, 0.02f, 0.02f, 1f);
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

        for (int i = 0; i < connectors.Count; i++)
        {
            Gizmos.color = new Color(0f, 1f, 0.17f, 0.56f);
            Gizmos.DrawCube(transform.position + new Vector3(connectors[i].x, connectors[i].y, 0), new Vector3(1f, 1f, 1f));
        }

    }
}