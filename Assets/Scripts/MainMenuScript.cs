using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public MapSelectionObject mapSelection;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        mapSelection.hardmodeEnabled = false;
        StartCoroutine(DelayStartGame());
    }

    public void StartGameMap2()
    {
        mapSelection.hardmodeEnabled = true;
        StartCoroutine(DelayStartGame());
    }

    public void QuitGame()
    {
        StartCoroutine(DelayQuitGame());
    }

    IEnumerator DelayStartGame()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("ChoosePlayerScene");
    }

    IEnumerator DelayQuitGame()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
}
