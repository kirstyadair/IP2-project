using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    StatsScript stats;
    public Text winningText;
    public Text playerStatsText;

    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.Find("StatsObject").GetComponent<StatsScript>();

        string txt = "";
        foreach (PlayerStats stat in stats.playerStats)
        {
            txt += "P" + stat.playerNumber + " got " + stat.kills + " and died " + stat.deaths + " times\n";
        }

        playerStatsText.text = txt;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) || (InputManager.ActiveDevice != null && InputManager.ActiveDevice.Action1.IsPressed))
        {
            GameObject selData = GameObject.Find("PlayerSelectionData");
            if (selData != null) Destroy(selData);

            Destroy(GameObject.Find("StatsObject"));


            SceneManager.LoadScene("ChoosePlayerScene");
        }
    }
}
