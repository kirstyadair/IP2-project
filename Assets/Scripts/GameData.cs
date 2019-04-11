using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { PREP, PLAY }
public enum GameWinner { PLAYERS, HORDE }

public class GameData : MonoBehaviour
{
    public MapSettings currentMap;

    // Triggered when the state is changed
    public delegate void StateChanged(GameState oldState, GameState newState);
    public event StateChanged OnStateChange;

    public delegate void GameOver(GameWinner winner);
    public event GameOver OnGameOver;

    // Current state
    public GameState state = GameState.PREP;

    public UIUpdater ui;

    // Current wave
    public int wave = 0;

    public HordeScript horde;

    public bool gameover = false;

    // Our countdown timer
    TimerScript timer;

    public WaveIndicatorScript waveIndicator;

    [Header("Pick player health here")]
    public int playerHealth;

    [Header("Wave indicator colours")]
    public Color preparingIndicatorFGColor;
    public Color preparingIndicatorBGColor;

    public Color spawningIndicatorFGColor;
    public Color spawningIndicatorBGColor;

    public Color playingIndicatorFGColor;
    public Color playingIndicatorBGColor;

    [Header("Damage values")]
    public int ketchupDamage;
    public int spatulaDamage;
    public int throwingKnifeDamage;
    public int fireExtinguisherDamage;
    public int cookerDamage;

    [Header("Player colours")]
    public Color[] playerColors;

    public int playersAlive = 0;

    private void Awake()
    {
        timer = GameObject.Find("Timer").GetComponent<TimerScript>();
        PlayerScript.OnPlayerDeath += OnPlayerDeath;
        this.OnGameOver += OnGAMEOVER;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject selData = GameObject.Find("PlayerSelectionData");
        if (selData != null) playerColors = selData.GetComponent<PlayerSelectionData>().playerColors;
        ChangeState(GameState.PREP);
    }

    // Change the state
    public void ChangeState(GameState newState)
    {
        if (newState == GameState.PREP)
        {
            playersAlive = 0;
            int seconds = currentMap.waves[0].secondsBeforeSpawning;

            // Initiate the countdown to start spawning
            timer.StartCountdown(seconds, StartSpawning);

            if (wave > 0)
            {
                waveIndicator.SwapText("PREPARING...", preparingIndicatorBGColor, preparingIndicatorFGColor);
            }
        }

        if (newState == GameState.PLAY)
        {
            waveIndicator.SwapText("SPAWNING...", spawningIndicatorBGColor, spawningIndicatorFGColor);
        }

        OnStateChange(state, newState);
        state = newState;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.PLAY && !gameover)
        {
            if (horde.isSpawning) waveIndicator.ChangeText("SPAWNING " + horde.zombiesAlive + "/" + horde.zombiesTotal);
            else waveIndicator.ChangeText("LEFT: " + horde.zombiesAlive + "/" + horde.zombiesTotal);

            if (horde.zombiesAlive == 0 && !horde.isSpawning)
            {

                // zombies are dead!
                // next wave
                wave++;

                Debug.Log("wave " + wave + " of " + currentMap.waves.Length);

                // all waves are done
                if (wave >= currentMap.waves.Length)
                {
                    
                    gameover = true;
                    OnGameOver(GameWinner.PLAYERS);
                }
                else
                {
                    ChangeState(GameState.PREP);
                }
            }
        }
    }

    void OnGAMEOVER(GameWinner winner)
    {
        // go to the end screen after a couple seconds
        StartCoroutine(DelayEndScreen());
    }

    IEnumerator DelayEndScreen()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("EndScene");
    }

    void OnPlayerDeath(PlayerScript player)
    {
        ui.ShowBigStatusBar("RIP " + player.chefType + " chef", 2f);

        if (playersAlive == 0)
        {
            gameover = true;
            OnGameOver(GameWinner.HORDE);
        }
    }


    public void StartSpawning()
    {
        // Start spawning from current spawn point
        Debug.Log("Starting spawning");

        horde.OnSpawnComplete += FinishedSpawning;
        ChangeState(GameState.PLAY);
    }

    public void FinishedSpawning()
    {
        waveIndicator.SwapText("PLAYING...", playingIndicatorBGColor, playingIndicatorFGColor);
    }
}
