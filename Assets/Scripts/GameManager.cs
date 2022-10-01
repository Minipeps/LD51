using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        StartScreen,
        Playing,
        GameOver,
    };
    // UI Bars
    public Slider healthBar;
    public Slider timerBar;

    public CoreBehaviour core;
    public MobSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Update UI
        healthBar.value = core.GetHealth();
        timerBar.value = 10 - spawner.GetTimeSinceLastSpawn(); 
    }
}
