using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveIndicatorScript : MonoBehaviour
{
    public Animator colourAnimator;
    public Image statusBarImage;
    public Text statusText;
    public Text waveNo;

    GameData gameData;

    public string text;
    public Color bgColor;
    public Color fgColor;

    bool isAnimating = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    public void ChangeText(string text)
    {
        this.text = text;
    }

    public void SwapText(string text, Color bgColor, Color fgColor)
    {
        this.text = text;
        this.bgColor = bgColor;
        this.fgColor = fgColor;
        colourAnimator.SetTrigger("swap");
        isAnimating = true;
    }

    void ReadyForSwap()
    {
        isAnimating = false;
    }

    // Update is called once per frame
    void Update()
    {
        waveNo.text = (gameData.wave + 1).ToString();

        if (!isAnimating)
        {
            statusText.color = fgColor;
            statusBarImage.color = bgColor;
            statusText.text = text;
        }
    }
}
