using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BugAI : MonoBehaviour
{
    [SerializeField]
    PlaceTile _tileManager;

    public SpriteRenderer healthBar;
    public SpriteRenderer body;

    public float movementSpeed = 10f;
    public float movementInertia = 200f;
    public int maxHealth = 20;
    public int attack = 10;

    Vector3 currentMovement;

    Vector3 _offset;

    int health;

    public UnityAction OnDeath;

    void Start()
    {
        _offset = _tileManager.tilemap.cellSize / 2;
        health = maxHealth;    
        healthBar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += currentMovement * Time.deltaTime * movementSpeed;
        body.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(currentMovement.y, currentMovement.x) - 90f);


        var index = FindClosestPathTile();
        if (index < CurrentPath().Count - 1)
            UpdateMovementData(_tileManager.tilemap.CellToWorld(CurrentPath()[index+1] ) + _offset - transform.position);
        else
            UpdateMovementData(Vector3.zero);
    }

    public void UpdateMovementData(Vector3 newMovement)
    {
        currentMovement = Vector3.Lerp(currentMovement, newMovement.normalized, movementInertia * Time.deltaTime);
    }
 
    public int GetScore()
    {
        return Mathf.RoundToInt(maxHealth * movementSpeed * attack);
    }

    public int GetCreditCost()
    {
        return GetScore() / 10;
    }

    public int GetReward()
    {
        return GetScore() / 100;
    }

    public List<Vector3Int> CurrentPath()
    {
        return _tileManager.GetCurrentPath();
    }

    public void SetTileManager(PlaceTile tileManager)
    {
        _tileManager = tileManager;
    }

    public void Hurt(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            OnDeath();
            Destroy(gameObject, 0.1f);
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.enabled = true;
        healthBar.transform.localScale = new Vector3( .9f * health / maxHealth, .1f, 1f);
    }

    private int FindClosestPathTile()
    {
        float bestDistance = 1e10f;
        int bestIndex = -1;
        for (int i = 0; i < CurrentPath().Count; ++i)
        {
            var tilePos = _tileManager.tilemap.CellToWorld(CurrentPath()[i]);
            float d = Vector3.Distance(transform.position, tilePos + _offset);
            if (d < bestDistance)
            {
                bestDistance = d;
                bestIndex = i;
            }
        }
        return bestIndex;
    }
}
