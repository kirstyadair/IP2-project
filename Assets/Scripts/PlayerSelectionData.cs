using InControl;
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
}
