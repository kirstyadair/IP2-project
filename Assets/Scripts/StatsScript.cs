using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public PlayerType playerType;
    public Color playerColor;
    public int playerNumber;
    public int deaths = 0;
    public int kills = 0;
}


// holds stats for players and the horde
public class StatsScript : MonoBehaviour
{
    public List<PlayerStats> playerStats = new List<PlayerStats>();

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // add these player stats to the list
    public void RegisterStats(PlayerScript playerScript)
    {
        PlayerStats stats = new PlayerStats();
        stats.playerType = playerScript.chefType;
        stats.playerColor = playerScript.playerColor;
        stats.playerNumber = playerScript.playerNumber;
        playerScript.stats = stats;

        playerStats.Add(stats);
    }

    // get the PlayerStats object for the given player
    public PlayerStats GetStats(PlayerScript playerScript)
    {
        foreach (PlayerStats stats in playerStats)
        {
            if (stats.playerNumber == playerScript.playerNumber) return stats;
        }

        throw new System.Exception("Couldn't find stats for player " + playerScript.playerNumber);
    }
}
