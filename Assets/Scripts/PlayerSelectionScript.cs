using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelectionScript : MonoBehaviour
{
    public Text playerList;
    public int playerCount = 0;
    public GameObject pressStartToBegin;
    public List<PlayerSelection> players = new List<PlayerSelection>();

    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData pointerEventData;

    PlayerSelectionData playerSelectionObject;
    bool starting = false;

    public bool playersAllActive = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSelectionObject = GameObject.Find("PlayerSelectionData").GetComponent<PlayerSelectionData>();
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (input)
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.tag == whateverTag)
                {
                    
                }
            }
        }*/

        if (Input.GetKey(KeyCode.E))
        {
            Destroy(GameObject.Find("PlayerSelectionData"));
            SceneManager.LoadScene("GameScene");
        }

        // Get the inputdevice that was most recently pressed
        InputDevice currentInput = InputManager.ActiveDevice;


        bool alreadyActive = false;

        

        // If X was pressed
        if (currentInput.Action1.WasPressed)
        {

            // Check to see if this player is already active
            foreach (PlayerSelection plr in players)
            {
                if (plr.input.GUID == currentInput.GUID) alreadyActive = true;
            }

            Debug.Log(alreadyActive);
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

                GameObject playerCursor = GameObject.Find((playerCount + 1).ToString());
                Color cursorColour = playerCursor.GetComponent<Image>().color;
                cursorColour.a = 1;
                playerCursor.GetComponent<Image>().color = cursorColour;
                playerCursor.GetComponent<CursorScript>().controller = currentInput;

                players.Add(newPlr);
                playerCount++;


            }
            
        }

        if (playerCount > 1 && alreadyActive)
        {
            if (currentInput.Action1.WasPressed)
            {
                // choose characters
                playersAllActive = true;

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
        //if (players.Count > 1)
        if (playersAllActive)
        {
            pressStartToBegin.SetActive(true);
            ChoosePlayerType();

            // If start button was pressed, assign the player types and start the game
            if (currentInput.Command.WasPressed && !starting)
            {
                starting = true;
                
                PreparePlayerSelectionObject();
                StartCoroutine(ChangeScene());
            }
        } 
    }

    void ChoosePlayerType()
    {
        PlayerType[] availableTypes = { PlayerType.THIN, PlayerType.FAT, PlayerType.CRAZY, PlayerType.HORDE };
        PlayerType currentPlayerType;
        bool canSelectChefs = true;
        bool fatChefSelected = false;
        bool thinChefSelected = false;
        bool crazyChefSelected = false;
        bool hordeSelected = false;


        // allow playerCount-1 people to choose a chef

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
