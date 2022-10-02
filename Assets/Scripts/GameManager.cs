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
    // End/Start screens
    public GameObject tutorialPanel;
    public GameObject gameOverPanel;
    public Button startButton;
    public Button resetButton;

    // UI Bars
    public Slider healthBar;
    public Slider timerBar;

    // Shop
    public Text bank;

    public Shop shop;
    public CoreBehaviour core;
    public MobSpawner spawner;
    public PlaceTile tileManager;

    // Game settings
    public int startCoins = 1000;

    public GameState state = GameState.Playing;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        resetButton.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Playing)
        {
            // Update UI
            healthBar.value = core.GetHealth() <= 0 ? 0 : core.GetHealth();
            healthBar.GetComponentInChildren<Text>().text = "Core Health: " + core.GetHealth() + " / 1000";
            timerBar.value = 10 - spawner.GetTimeSinceLastSpawn();
            timerBar.GetComponentInChildren<Text>().text = "Next: Wave " + spawner.GetWaveNumber();

            bank.text = shop.GetBank() == 0 ? "0" : shop.GetBank().ToString("###,###");

            if (core.GetHealth() <= 0)
            {
                GameOver();
            }
        }

    }

    private void StartGame()
    {
        state = GameState.Playing;
        tutorialPanel.SetActive(false);
        shop.SetBank(startCoins);
        spawner.ResetGame();
        tileManager.EnablePlacement();
        core.ResetHealth();
    }

    private void GameOver()
    {
        state = GameState.GameOver;
        spawner.StopGame();
        gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        state = GameState.StartScreen;
        spawner.KillAllBugs();
        tileManager.ResetMap();
        gameOverPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }
}
