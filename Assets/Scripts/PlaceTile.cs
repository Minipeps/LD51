using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceTile : MonoBehaviour
{
    public Tile selectedTile;
    public Tile backgroundTile;

    public GameObject tilePrefab;
    public Tilemap tilemap;


    int TILESIZE = 32;


    // Start is called before the first frame update
    void Start()
    {   
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
        }
    }

    public bool placeTile()
    {
        //// TODO: check whether a tile can be placed at that emplacement
        var tileCoord = getHoveredWorldCoord();

        tilemap.SetTile(tilemap.WorldToCell(tileCoord), selectedTile);

        return true;
    }

    public bool deleteTile()
    {
        var tileCoord = getHoveredWorldCoord();
        tilemap.SetTile(tilemap.WorldToCell(tileCoord), backgroundTile);
        return true;
    }


    private Vector3 getHoveredWorldCoord()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    } 
}
