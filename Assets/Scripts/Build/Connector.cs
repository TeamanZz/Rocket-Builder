using UnityEngine;

public class Connector : MonoBehaviour
{
    [Header("ItemSettings")]
    [SerializeField] public Vector2Int Size = Vector2Int.one;
    [SerializeField] private BuildingGrid buildingGrid;


    private void Awake()
    {
        buildingGrid = FindObjectOfType<BuildingGrid>();
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Gizmos.color = new Color(0f, 1f, 0.12f, 0.56f);
                Gizmos.DrawCube(transform.position + new Vector3(x, y, 0), new Vector3(1f, 1f, 1f));
            }
        }
    }
}
