using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject playerInput;
    public GameObject aboutScreen;

    public bool pressPlay = false;

    public void StartGame()
    {
        titleScreen.gameObject.SetActive(false);
        playerInput.GetComponent<TMP_InputField>().placeholder.GetComponent<TMP_Text>().text = "ENTER YOUR NAME CHALLENGER..."; //Must grab placeholder element here since UI doesn't have it.
        playerInput.gameObject.SetActive(true);
        pressPlay = true;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && pressPlay)
        {
            NameSubmitted();
        }
    }

    public void NameSubmitted()
    {
        SetName();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads next scene based on build order.
    }

    public void QuitGame()
    {
        Debug.Log("Application Closed"); //Set for testing purposes since executable is needed.
        Application.Quit();
    }

    public void AboutGame()
    {
        titleScreen.gameObject.SetActive(false);
        aboutScreen.gameObject.SetActive(true);
    }

    public void BackButton()
    {
        aboutScreen.gameObject.SetActive(false);
        titleScreen.gameObject.SetActive(true);
    }

    public void SetName()
    {
        string playerName = playerInput.GetComponent<TMP_InputField>().text;
        if(playerName == "")
        {
            playerName = "Pacman";
        }
        PlayerPrefs.SetString("Name", playerName); 
    }
}
