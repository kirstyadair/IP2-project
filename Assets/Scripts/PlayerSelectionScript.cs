using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelectionScript : MonoBehaviour
{
    public GameObject someoneHasToPickHorde;
    public GameObject pressOptionsToStart;
    public int playerCount = 0;
    public GameObject pressStartToBegin;
    public List<PlayerSelection> players = new List<PlayerSelection>();

    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData pointerEventData;

    PlayerSelectionData playerSelectionObject;
    bool starting = false;

    public ChoosePlayerSelector fatChef;
    public ChoosePlayerSelector thinChef;
    public ChoosePlayerSelector crazyChef;
    public ChoosePlayerSelector horde;

    public bool readyToStart = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSelectionObject = GameObject.Find("PlayerSelectionData").GetComponent<PlayerSelectionData>();
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        ChoosePlayerSelector.OnSelectionChanged += OnPlayerSelectionChanged;
    }

    public void OnDestroy()
    {
        ChoosePlayerSelector.OnSelectionChanged -= OnPlayerSelectionChanged;
    }

    public bool IsHordeSelected()
    {
        foreach (PlayerSelection selection in players)
        {
            if (selection.playerType == PlayerType.HORDE) return true;
        }

        return false;
    }
    
    public void OnPlayerSelectionChanged(ChoosePlayerSelector selector)
    {
        RefreshPlayerAvailabilities();
    }

    // Force a player to pick the horde
    public void RefreshPlayerAvailabilities()
    {
        // If the horde hasn't been selected yet AND there is one player still left to pick, force them to pick the horde by deactivating the chefs
        if (players.Count > 1 && !horde.isSelected && PlayersThatHaveSelected() == players.Count - 1)
        {
            fatChef.Deactivate();
            thinChef.Deactivate();
            crazyChef.Deactivate();
            someoneHasToPickHorde.SetActive(true);
        } else
        {
            fatChef.Activate();
            thinChef.Activate();
            crazyChef.Activate();
            someoneHasToPickHorde.SetActive(false);
        }

        if (PlayersThatHaveSelected() == players.Count && players.Count > 1)
        {
            pressOptionsToStart.SetActive(true);
            readyToStart = true;
        } else
        {
            pressOptionsToStart.SetActive(false);
            readyToStart = false;
        }
    }

    public int PlayersThatHaveSelected()
    {
        int results = 0;
        foreach (PlayerSelection selection in players) if (selection.playerType != PlayerType.UNDECIDED) results++;

        return results;
    }

    // Update is called once per frame
    void Update()
    {
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

                CursorScript cursorScript = playerCursor.GetComponent<CursorScript>();
                playerCursor.GetComponent<Image>().color = cursorColour;

                cursorScript.player = newPlr;
                cursorScript.Yeet();
                cursorScript.playerNumber = playerCount;

                playerCursor.transform.SetParent(transform); // pull the cursor out of the playerindicator gameobject 
                players.Add(newPlr);
                playerCount++;

                RefreshPlayerAvailabilities();
            }
            
        }


        if (readyToStart)
        {
            // If start button was pressed, start the game
            if (currentInput.Command.WasPressed && !starting)
            {
                starting = true;
                
                PreparePlayerSelectionObject();
                SceneManager.LoadScene("GameScene");
            }
        } 
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
