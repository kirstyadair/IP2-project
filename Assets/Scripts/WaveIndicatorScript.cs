using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveIndicatorScript : MonoBehaviour
{
    public Animator colourAnimator;
    public Text statusText;
    public Text waveNo;

    GameData gameData;
    // Start is called before the first frame update
    void Awake()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        waveNo.text = (gameData.wave + 1).ToString();
    }
}
