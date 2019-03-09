using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection
{
    public InputDevice input;
    public PlayerType playerType;
}

public class PlayerSelectionData : MonoBehaviour
{
    public List<PlayerSelection> playerSelections;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InputDevice GetHordeController()
    {
        foreach (PlayerSelection plr in playerSelections)
        {
            if (plr.playerType == PlayerType.HORDE) return plr.input;
        }

        return null;
    }
}
