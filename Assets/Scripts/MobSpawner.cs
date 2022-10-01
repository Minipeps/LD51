using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobSpawner : MonoBehaviour
{
    int SPAWN_TIMER = 10;

    public GameObject mob;
    public GameObject core;
    public PlaceTile tileManager;

    float lastSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
         Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (lastSpawnTime > SPAWN_TIMER)
        {
            Spawn();
            lastSpawnTime = 0;
        }
        else
        {
            lastSpawnTime += Time.deltaTime;
            //Debug.Log(lastSpawnTime);
        }
    }


    public float GetTimeSinceLastSpawn()
    {
        return lastSpawnTime;
    }

    private void Spawn()
    {
        var newMob = Instantiate(mob, gameObject.transform).GetComponent<BugAI>();
        newMob.SetTarget(core);
        newMob.SetTileManager(tileManager);

        //newMob.GetComponent<BugAI>().UpdateMovementData( (core.transform.position - mob.transform.position).normalized);
    }
}
