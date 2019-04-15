using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    StatsScript stats;
    GameData gameData;
    public Text winningText;
    public Text playerStatsText;
    public Sprite hordeWinBG;
    public Sprite chefsWinBG;
    public Image background;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        stats = GameObject.Find("StatsObject").GetComponent<StatsScript>();

        string txt = "";
        foreach (PlayerStats stat in stats.playerStats)
        {
            txt += "P" + stat.playerNumber + " got " + stat.kills + " and died " + stat.deaths + " times\n";
        }

        playerStatsText.text = txt;

        if (gameData.winner == GameWinner.HORDE)
        {
            Debug.Log("Sushi won");
            background.GetComponent<Image>().sprite = hordeWinBG;
        }
        else
        {
            Debug.Log("Chefs won");
            background.GetComponent<Image>().sprite = chefsWinBG;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) || (InputManager.ActiveDevice != null && InputManager.ActiveDevice.Action1.IsPressed))
        {
            GameObject selData = GameObject.Find("PlayerSelectionData");
            if (selData != null) Destroy(selData);

            Destroy(GameObject.Find("StatsObject"));
            Destroy(GameObject.Find("MapSelectionObject"));
            Destroy(GameObject.Find("GameData"));

            SceneManager.LoadScene("StartScene");
        }
    }
}
