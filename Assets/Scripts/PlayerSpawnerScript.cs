using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerScript : MonoBehaviour
{
    public HealthbarFactory healthbars;
    GameData gameData;
    List<GameObject> playerSpawnPoints;
    PlayerSelectionData playerSelectionData;
    public GameObject fatChefPrefab;
    public GameObject thinChefPrefab;
    public GameObject crazyChefPrefab;

    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();

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
            SpawnPlayerAt(playerSpawnPoints[i], playerSelection.playerType, playerSelection.input, i + 1, gameData.playerColors[i]);
            i++;
        }
    }

    void SpawnPlayerAt(GameObject spawnPoint, PlayerType playerType, InputDevice input, int playerNumber, Color playerColor)
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

        newPlayer.GetComponent<PlayerScript>().playerNumber = playerNumber;
        newPlayer.GetComponent<PlayerScript>().playerColor = playerColor;

        if (healthbars != null) healthbars.CreateHealthbar(newPlayer.GetComponent<PlayerScript>());
    }

    void SpawnPlayersForKeyboardAndMouseControl()
    {
        SpawnPlayerAt(playerSpawnPoints[0], PlayerType.FAT, null, 1, gameData.playerColors[0]);
        SpawnPlayerAt(playerSpawnPoints[1], PlayerType.CRAZY, null, 2, gameData.playerColors[1]);
        SpawnPlayerAt(playerSpawnPoints[2], PlayerType.THIN, null, 3, gameData.playerColors[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
