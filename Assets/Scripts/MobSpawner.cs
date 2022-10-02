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
    int bossWaves = 0;

    List<GameObject> bugs = new List<GameObject>();

    public bool running = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!running)
            return;

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

    public void StopGame()
    {
        tileManager.DisablePlacement();
        running = false;
    }

    public void ResetGame()
    {
        running = true;
        waveNumber = 1;
        bossWaves = 0;
        lastSpawnTime = 0;
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

    public void KillAllBugs()
    {
        foreach(var bug in bugs)
        {
            Destroy(bug, 0.1f);
        }
        bugs.Clear();
    }

    private void SpawnNewBug(GameObject bugPrefab)
    {
        var newMob = Instantiate(bugPrefab, gameObject.transform).GetComponent<BugAI>();
        newMob.SetTileManager(tileManager);
        newMob.OnDeath += () => shop.AddToBank(newMob.GetReward());
        bugs.Add(newMob.gameObject);
    }

    private List<int> GenerateWave()
    {
        var waveCredits = Mathf.RoundToInt( Mathf.Sqrt( waveNumber ) * 100 * Mathf.Log10(waveNumber)) + 100 + bossWaves * 2000;
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

        if (waveNumber > 0 && (waveNumber % 25) == 0)
        {
            Debug.Log("Boss wave");
            bossWaves++;
            indices.Add(3);
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
