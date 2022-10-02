using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        StartScreen,
        Playing,
        GameOver,
    };
    // UI Bars
    public Slider healthBar;
    public Slider timerBar;

    // Shop
    public Text bank;

    public Shop shop;
    public CoreBehaviour core;
    public MobSpawner spawner;

    // Game settings
    public int startCoins = 1000;

    public GameState state = GameState.Playing;

    // Start is called before the first frame update
    void Start()
    {
        shop.SetBank(startCoins);
    }

    // Update is called once per frame
    void Update()
    {
        // Update UI
        healthBar.value = core.GetHealth() <= 0 ? 0 : core.GetHealth();
        healthBar.GetComponentInChildren<Text>().text = "Health: " + core.GetHealth() + " / 1000";
        timerBar.value = 10 - spawner.GetTimeSinceLastSpawn();
        timerBar.GetComponentInChildren<Text>().text = "Next: Wave " + spawner.GetWaveNumber();

        bank.text = shop.GetBank() == 0 ? "0" : shop.GetBank().ToString("###,###");

        if (core.GetHealth() <= 0)
        {
            state = GameState.GameOver;
            // TODO: handle restart
        }
    }
}
