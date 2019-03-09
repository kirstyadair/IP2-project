using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelectionScript : MonoBehaviour
{
    public Text playerList;
    public GameObject pressStartToBegin;
    public List<PlayerSelection> players = new List<PlayerSelection>();

    PlayerSelectionData playerSelectionObject;
    bool starting = false;
    // Start is called before the first frame update
    void Start()
    {
        playerSelectionObject = GameObject.Find("PlayerSelectionData").GetComponent<PlayerSelectionData>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the inputdevice that was most recently pressed
        InputDevice currentInput = InputManager.ActiveDevice;

        // If X was pressed
        if (currentInput.Action1.WasPressed)
        {
            Debug.Log("Input " + currentInput.GUID + " pressed X");

            // Check to see if this player is already active
            bool alreadyActive = false;

            foreach (PlayerSelection plr in players)
            {
                Debug.Log("Comparing " + currentInput.GUID + " against " + plr.input.GUID + " - result: " + (plr.input.GUID == currentInput.GUID));
                if (plr.input.GUID == currentInput.GUID) alreadyActive = true;
            }

            // Don't add any more than 4 players
            if (players.Count >= 4) alreadyActive = true;

            if (!alreadyActive)
            {
                // Add the new player
                PlayerSelection newPlr = new PlayerSelection
                {
                    playerType = PlayerType.UNDECIDED,
                    input = currentInput
                };

                players.Add(newPlr);
            }
        }

        playerList.text = "";

        // Draw the player list
        int i = 0;
        foreach (PlayerSelection plr in players)
        {
            i++;
            playerList.text += "Player " + i + " - " + plr.playerType + "\n";
        }

        // If there is more than one player, give option to start game
        if (players.Count > 1)
        {
            pressStartToBegin.SetActive(true);

            // If start button was pressed, assign the player types and start the game
            if (currentInput.Command.WasPressed && !starting)
            {
                starting = true;
                AssignPlayers();
                PreparePlayerSelectionObject();
                StartCoroutine(ChangeScene());
            }
        } 
    }

    void AssignPlayers()
    {
        foreach (PlayerSelection plr in players) plr.playerType = PlayerType.UNDECIDED;

        // Available player types 
        PlayerType[] availableTypes = { PlayerType.CRAZY, PlayerType.THIN, PlayerType.FAT, PlayerType.HORDE };

        // Randomly pick a unique typ per player
        foreach (PlayerSelection plr in players)
        {
            bool alreadyPicked = false;
            PlayerType randomChef = PlayerType.UNDECIDED;
            do
            {
                // Keep picking random types until it's unqiue
                alreadyPicked = false;
                randomChef = availableTypes[Random.Range(0, availableTypes.Length)];
                foreach (PlayerSelection player in players) if (player.playerType == randomChef) alreadyPicked = true;
            } while (alreadyPicked);

            plr.playerType = randomChef;
        }

        // Check if one of the players is already assigned as the horde, otherwise pick a random one to be
        bool hordePicked = false;

        foreach (PlayerSelection plr in players) if (plr.playerType == PlayerType.HORDE) hordePicked = true;

        if (!hordePicked) players[Random.Range(0, players.Count)].playerType = PlayerType.HORDE;
    }

    // Pass the player selection details to the PlayerSelectionObject to be persisted into the GameScene
    void PreparePlayerSelectionObject()
    {
        playerSelectionObject.playerSelections = players;
    }

    IEnumerator ChangeScene()
    {
        // Start the game after a second
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameScene");
    }
}
