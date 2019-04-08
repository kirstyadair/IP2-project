using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // for the love of god, remember to attach this to the fucking gameobject
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice currentInput = InputManager.ActiveDevice;

        if (currentInput.LeftStick.Left)
        {
            Debug.Log("LEEEEEEEFT");
        }
    }
}
