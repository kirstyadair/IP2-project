using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just carries map selection data to the game
public class MapSelectionObject : MonoBehaviour
{
    public bool hardmodeEnabled = false;
    
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
