using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// TODO: rename to TileManager
public class PlaceTile : MonoBehaviour
{
    public GameObject spawn;
    public GameObject core;

    public Tile selectedTile;
    public Tile backgroundTile;
    public Tilemap tilemap;

    public Tilemap debugTilemap;
    public Tile debugTile;

    public Vector2Int topLeftBounds;
    public Vector2Int bottomRightBounds;

    NavigateTilemap navigator;

    List<Vector3Int> currentPath;

    // Start is called before the first frame update
    void Awake()
    {
        navigator = tilemap.GetComponent<NavigateTilemap>();
    }
    void Start()
    {
        RefreshPath(spawn.transform.position, core.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            placeTile();
        }
       else if (Input.GetMouseButtonDown(1))
        {
            deleteTile();
            RefreshPath(spawn.transform.position, core.transform.position);
        }
    }
    public bool RefreshPath(Vector3 start, Vector3 end)
    {
        var startCell = tilemap.WorldToCell(start);
        var endCell = tilemap.WorldToCell(end);
        bool reached = false;
        var newPath = navigator.navigate(startCell, endCell, out reached);
        // If a new path is resolved, use it
        if (reached)
        {
            currentPath = newPath;
            DrawDebugPath();
            return true;
        }
        // Otherwise keep the old path
        return false;
    }

    public List<Vector3Int> GetCurrentPath()
    {
        return currentPath;
    }

    public bool placeTile()
    {
        var tileCoord = getHoveredWorldCoord();

        var cellCoord = tilemap.WorldToCell(tileCoord);


        if (!isInPlayableArea(cellCoord))
            return false;

        tilemap.SetTile(cellCoord, selectedTile);

        // Check whether a tile can be placed at that emplacement
        if (!RefreshPath(spawn.transform.position, core.transform.position))
        {
            Debug.Log("Path is blocked, aborting tile placement");
            deleteTile();
            return false;
        }

        Debug.Log("Placed tile at " + cellCoord);
        return true;
    }

    public void deleteTile()
    {
        var tileCoord = getHoveredWorldCoord();

        var cellCoord = tilemap.WorldToCell(tileCoord);
        Debug.Log("Clicked at " + cellCoord);

        if (!isInPlayableArea(cellCoord))
            return;

        tilemap.SetTile(cellCoord, backgroundTile);
    }


    private Vector3 getHoveredWorldCoord()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool isInPlayableArea(Vector3Int cellCoord)
    {
        return cellCoord.x > topLeftBounds.x && cellCoord.x < bottomRightBounds.x && cellCoord.y < topLeftBounds.y && cellCoord.y > bottomRightBounds.y;
    }

    private void DrawDebugPath()
    {
        debugTilemap.ClearAllTiles();
        foreach (var pos in currentPath)
        {
            debugTilemap.SetTile(pos, debugTile);
        }
    }
}
