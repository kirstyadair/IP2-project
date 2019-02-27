using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { PREP, PLAY }

public class GameData : MonoBehaviour
{
    public MapSettings currentMap;
    public EntryPointScript currentEntryPoint;

    // Triggered when the state is changed suprisingly
    public delegate void StateChanged(GameState oldState, GameState newState);
    public event StateChanged OnStateChange;

    // Current state
    public GameState state = GameState.PREP;

    // Current wave
    public int wave = 0;

    // Current chosen spawn point
    SpawnPointScript currentSpawnPoint;

    // Our countdown timer
    TimerScript timer;

    private void Awake()
    {
        timer = GameObject.Find("Timer").GetComponent<TimerScript>();
        Debug.Log(timer);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.PREP);
    }

    // Change the state
    public void ChangeState(GameState newState)
    {
        if (newState == GameState.PREP)
        {
            int seconds = currentMap.waves[0].secondsBeforeSpawning;

            // Initiate the countdown to start spawning
            timer.StartCountdown(seconds, StartSpawning);
        }

        OnStateChange(state, newState);
        state = newState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        // Start spawning from current spawn point
        Debug.Log("Starting spawning");
    }

    public void SetEntryPoint(EntryPointScript entryPointScript)
    {
        currentEntryPoint = entryPointScript;
    }
}
