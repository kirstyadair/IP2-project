using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    // Status bar at bottom that shows "Pick entry point"
    public Animator bigStatusBarAnimator;
    public GameObject slider;
    public GameData gameData;
    public Animator sliderAnimator;

    public string hordeIncomingText = "HORDE INC[OMI]NG";
    public float hordeIncomingAppearFor = 2;

    public string getReadyText = "GET READY";
    public float getReadyAppearFor = 3;

    // Start is called before the first frame update
    void Awake()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        gameData.OnStateChange += StateChange;
        gameData.OnGameOver += OnGameOver;
    }

    public void StateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.PREP)
        {
            ShowBigStatusBar(getReadyText, getReadyAppearFor);
        }
        if (newState == GameState.PLAY)
        {
            ShowBigStatusBar(hordeIncomingText, hordeIncomingAppearFor);
        }
    }

    public void OnGameOver(GameWinner winner)
    {
        sliderAnimator.Play("sliderin");
    }

    public void ShowBigStatusBar(string text, float secondsToShow)
    {
        bigStatusBarAnimator.gameObject.transform.Find("Text").GetComponent<Text>().text = text;
        bigStatusBarAnimator.SetBool("showing", true);
        StartCoroutine(CloseStatusBar(secondsToShow));
    }

    IEnumerator CloseStatusBar(float secondsToShow)
    {
        yield return new WaitForSeconds(secondsToShow);
        bigStatusBarAnimator.SetBool("showing", false);
    }

    public void Start()
    {
        slider.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
