using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevSettings : MonoBehaviour
{
    public GameObject hordeSettings;
    public GameObject playerSettings;
    public Text hideText;
    public bool hidden = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace)) hidden = !hidden;

        if (hidden)
        {
            hideText.text = "Backspace to show dev settings";
            hordeSettings.SetActive(false);
            playerSettings.SetActive(false);
        } else
        {
            hideText.text = "Backspace to hide dev settings";
            hordeSettings.SetActive(true);
            playerSettings.SetActive(true);
        }
    }
}
