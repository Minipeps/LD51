using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobSpawner : MonoBehaviour
{
    int SPAWN_TIMER = 10;

    public GameObject[] bugPrefabs;
    public GameObject core;
    public PlaceTile tileManager;
    public Shop shop;

    float lastSpawnTime = 0;
    int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSpawnTime > SPAWN_TIMER)
        {
            SpawnWave();
            lastSpawnTime = 0;
        }
        else
        {
            lastSpawnTime += Time.deltaTime;
        }
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetTimeSinceLastSpawn()
    {
        return lastSpawnTime;
    }

    private void SpawnWave()
    {
        List<int> bugsToSpawn = GenerateWave();
        StartCoroutine(SpawnBugs(bugsToSpawn));
        
        waveNumber++;
    }

    private void SpawnNewBug(GameObject bugPrefab)
    {
        var newMob = Instantiate(bugPrefab, gameObject.transform).GetComponent<BugAI>();
        newMob.SetTileManager(tileManager);
        newMob.OnDeath += () => shop.AddToBank(newMob.GetReward());
    }

    private List<int> GenerateWave()
    {
        var waveCredits = Mathf.RoundToInt( Mathf.Log10( waveNumber * 30 ) * 50);
        var spentCredits = 0;
        
        List<int> indices = new List<int>();

        List<int> indexPool = GetIndexPool();
        while (spentCredits < waveCredits && indexPool.Count > 0)
        {
            int index = indexPool[Random.Range(0, indexPool.Count)];
            int reward = bugPrefabs[index].GetComponent<BugAI>().GetCreditCost();
            if (spentCredits + reward < waveCredits)
            {
                spentCredits += reward;
                indices.Add(index);
                indexPool = GetIndexPool();
            }
            else
            {
                indexPool.Remove(index);
            }
        }
        Debug.Log("Credits: available " + waveCredits + ", spent " + spentCredits);
        return indices;
    }

    private List<int> GetIndexPool()
    {
        List<int> pool = new List<int>();
        // Base bugs are more common
        pool.Add(0);
        pool.Add(0);
        for (int i = 1; i < bugPrefabs.Length; ++i)
            pool.Add(i);

        return pool;
    }

    IEnumerator SpawnBugs(List<int> bugsToSpawn)
    {
        foreach (int bugType in bugsToSpawn)
        {
            SpawnNewBug(bugPrefabs[bugType]);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
