using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BugAI : MonoBehaviour
{
    [SerializeField]
    GameObject _target;
    [SerializeField]
    PlaceTile _tileManager;

    public float movementSpeed = 10f;
    public float movementInertia = 1000f;
    public int health = 100;

    Vector3 currentMovement;
    Vector3 currentLook;

    float UPDATE_PATH_TIME = 0.01f;
    float timeSinceLastUpdate = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += currentMovement * Time.deltaTime * movementSpeed;
        // TODO: look toward moving direction

        if (timeSinceLastUpdate > UPDATE_PATH_TIME)
        {
            timeSinceLastUpdate = 0;
            var index = FindClosestPathTile();

            if (index < CurrentPath().Count - 1)
                UpdateMovementData(CurrentPath()[index + 1] - CurrentPath()[index]);
        }
        else
        {
            timeSinceLastUpdate += Time.deltaTime;
        }
    }

    public void UpdateMovementData(Vector3 newMovement)
    {
        currentMovement = Vector3.Lerp(currentMovement, newMovement.normalized, movementInertia * Time.deltaTime);
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public List<Vector3Int> CurrentPath()
    {
        return _tileManager.GetCurrentPath();
    }

    public void SetTileManager(PlaceTile tileManager)
    {
        _tileManager = tileManager;
    }

    private int FindClosestPathTile()
    {
        float bestDistance = 1e10f;
        int bestIndex = -1;
        for (int i = 0; i < CurrentPath().Count; ++i)
        {
            var tilePos = _tileManager.tilemap.CellToWorld(CurrentPath()[i]);
            float d = Vector3.Distance(transform.position, tilePos);
            if (d < bestDistance)
            {
                bestDistance = d;
                bestIndex = i;
            }
        }
        return bestIndex;
    }
}
