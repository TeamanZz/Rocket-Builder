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
    public List<Vector2> connectors = new List<Vector2>();

    private BuildingGrid buildingGrid;

    public bool isConnected;

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