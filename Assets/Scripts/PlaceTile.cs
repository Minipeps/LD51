using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// TODO: rename to TileManager
public class PlaceTile : MonoBehaviour
{
    enum PlacementType
    {
        Tile,
        Turret
    };
    public GameObject spawn;
    public GameObject core;
    public Shop shop;

    public GameObject selectedTurret;

    public Tile selectedTile;
    public Tile backgroundTile;
    public Tilemap tilemap;

    public Tilemap debugTilemap;
    public Tile debugTile;

    public Vector2Int topLeftBounds;
    public Vector2Int bottomRightBounds;

    NavigateTilemap navigator;

    List<Vector3Int> currentPath;

    PlacementType placementType = PlacementType.Tile;

    Dictionary<Vector3Int, GameObject> turrets = new Dictionary<Vector3Int, GameObject>();

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
            bool didBuy = false;
            if (placementType == PlacementType.Tile)
            {
                didBuy = placeTile();
            }
            else if (placementType == PlacementType.Turret)
            {
                didBuy = placeTurret();
            }
            if (didBuy)
                shop.BuyItem();
        }
       else if (Input.GetMouseButtonDown(1))
        {
            bool didSell = false;
            if (placementType == PlacementType.Tile)
            {
                didSell = deleteTile();
                RefreshPath(spawn.transform.position, core.transform.position);
            }
            else if (placementType == PlacementType.Turret)
            {
                didSell = deleteTurret();
            }
            if (didSell)
                shop.SellItem();
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

    public void SelectTile(Tile newTile)
    {
        selectedTile = newTile;
        placementType = PlacementType.Tile;

    }

    public void SelectTurret(GameObject newTurret)
    {
        placementType = PlacementType.Turret;
        selectedTurret = newTurret;
    }


    public bool placeTile()
    {
        var tileCoord = getHoveredWorldCoord();

        var cellCoord = tilemap.WorldToCell(tileCoord);


        if (!isInPlayableArea(cellCoord) || !shop.CanAffordCurrentItem())
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

    public bool deleteTile()
    {
        var tileCoord = getHoveredWorldCoord();

        var cellCoord = tilemap.WorldToCell(tileCoord);
        Debug.Log("Clicked at " + cellCoord);

        if (!isInPlayableArea(cellCoord))
            return false;

        // Check tile can be deleted
        var tile = tilemap.GetTile<Tile>(cellCoord);
        if (tile.colliderType == Tile.ColliderType.None || turrets.ContainsKey(cellCoord))
            return false;

        tilemap.SetTile(cellCoord, backgroundTile);
        return true;
    }

    public bool placeTurret()
    {
        var tileCoord = getHoveredWorldCoord();

        var cellCoord = tilemap.WorldToCell(tileCoord);

        if (!isInPlayableArea(cellCoord) || ! shop.CanAffordCurrentItem())
            return false;

        // Check tile can get a turret
        var tile = tilemap.GetTile<Tile>(cellCoord);
        if (tile.colliderType == Tile.ColliderType.None)
        {
            Debug.Log("Turret must be placed on a wall!");
            return false;
        }

        Vector3 offset = tilemap.cellSize / 2;
        var turret = Instantiate(selectedTurret, tilemap.CellToWorld(cellCoord) + offset, Quaternion.identity);
        turrets.Add(cellCoord, turret);
        return true;
    }

    public bool deleteTurret()
    {
        var tileCoord = getHoveredWorldCoord();

        var cellCoord = tilemap.WorldToCell(tileCoord);

        if (!isInPlayableArea(cellCoord))
            return false;

        GameObject turret;
        bool deleted = turrets.Remove(cellCoord, out turret);
        Destroy(turret, 0.1f);
        return deleted;
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
