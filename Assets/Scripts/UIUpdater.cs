using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    // Status bar at bottom that shows "Pick entry point"
    public Animator bigStatusBarAnimator;
    public GameData gameData;

    public string hordeIncomingText = "HORDE INC[OMI]NG";
    public float hordeIncomingAppearFor = 2;

    public string pickEntryPointText = "HORDE: PICK ENTRY POINT";
    public float pickEntryPointAppearFor = 3;

    // Start is called before the first frame update
    void Awake()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        gameData.OnStateChange += StateChange;
    }

    public void StateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.PREP)
        {
            ShowBigStatusBar(pickEntryPointText, pickEntryPointAppearFor);
        }
        if (newState == GameState.PLAY)
        {
            ShowBigStatusBar(hordeIncomingText, hordeIncomingAppearFor);
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
