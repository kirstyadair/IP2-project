using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerScript : MonoBehaviour
{
    List<GameObject> playerSpawnPoints;
    PlayerSelectionData playerSelectionData;
    public GameObject fatChefPrefab;
    public GameObject thinChefPrefab;
    public GameObject crazyChefPrefab;

    void Start()
    {
        // This will only find the currently active spawn points
        playerSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player spawnpoint"));

        GameObject selectiomDataObj = GameObject.Find("PlayerSelectionData");
        if (selectiomDataObj != null)
        {
            playerSelectionData = selectiomDataObj.GetComponent<PlayerSelectionData>();
            SpawnPlayers();
        }
        else SpawnPlayersForKeyboardAndMouseControl();

    }

    void SpawnPlayers()
    {
        int i = 0;
        foreach (PlayerSelection playerSelection in playerSelectionData.playerSelections)
        {
            SpawnPlayerAt(playerSpawnPoints[i], playerSelection.playerType, playerSelection.input);
            i++;
        }
    }

    void SpawnPlayerAt(GameObject spawnPoint, PlayerType playerType, InputDevice input)
    {
        GameObject chefPrefab = null;

        switch (playerType)
        {
            case PlayerType.FAT:
                chefPrefab = fatChefPrefab;
                break;
            case PlayerType.THIN:
                chefPrefab = thinChefPrefab;
                break;
            case PlayerType.CRAZY:
                chefPrefab = crazyChefPrefab;
                break;
        }

        if (chefPrefab == null) return; // Don't spawn the HORDE type

        // Spawn the new player
        GameObject newPlayer = Instantiate(chefPrefab);
        newPlayer.name = playerType.ToString() + " player";

        // Place it at the spawn point
        newPlayer.transform.position = spawnPoint.transform.position;

        // Assign the controller
        newPlayer.GetComponent<PlayerScript>().controller = input;
    }

    void SpawnPlayersForKeyboardAndMouseControl()
    {
        SpawnPlayerAt(playerSpawnPoints[0], PlayerType.FAT, null);
        SpawnPlayerAt(playerSpawnPoints[1], PlayerType.CRAZY, null);
        SpawnPlayerAt(playerSpawnPoints[2], PlayerType.THIN, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
