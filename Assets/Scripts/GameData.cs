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

    public delegate void ZombiePopEvent();
    public event ZombiePopEvent OnReadyForZombies;

    // Current state
    public GameState state = GameState.PREP;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.PREP);
    }

    // Change the state
    public void ChangeState(GameState newState)
    {
        if (newState == GameState.PLAY)
        {
            StartCoroutine(DelayZombiesReadyToPop());
        }

        OnStateChange(state, newState);
        state = newState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayZombiesReadyToPop()
    {
        yield return new WaitForSeconds(3f);
        OnReadyForZombies();
    }

    public void SetEntryPoint(EntryPointScript entryPointScript)
    {
        currentEntryPoint = entryPointScript;
    }
}
