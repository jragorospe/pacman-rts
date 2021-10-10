using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public GameObject pauseButton;
    public GameObject retryButton;
    public GameObject pauseMenu;
    public GameObject aboutScreen;
    public GameObject winScreen;
    public GameObject UIScreen;
    public GameObject scoreObject;

    static AudioSource audioSource;
    public AudioClip celebration;
    public AudioClip winningTheme;

    public CameraController cameraController;
    public ZoomController zoomController;

    public string playerName;
    public int score;
    public bool isWin = false;

    void Start()
    {
        StartCoroutine(DisplayObjective());
        audioSource = gameObject.GetComponent<AudioSource>(); //Starts music.
        playerName = PlayerPrefs.GetString("Name"); //Player Prefs carries save data across scenes.
        nameText.SetText("Player\n" + playerName);
        score = 6;
        scoreText.SetText("Units Needed: " + score);
    }

    void Update()
    {
        scoreText.SetText("Units Needed: " + score);

        if(score == 0 && !isWin)
        {
            isWin = true; //Prevents this being called every frame once you win.
            StartCoroutine(WinScreen());
            UIScreen.gameObject.SetActive(false);
            Camera.main.GetComponent<AudioSource>().Pause(); //Turn music off.
            audioSource.PlayOneShot(winningTheme);
        }
    }

    public void PauseButton()
    { 
        //Disables HUD and only shows pause menu. 
        cameraController.DisableInput();
        zoomController.DisableInput();

        UIScreen.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true); 

        Camera.main.GetComponent<AudioSource>().Pause();
    }

    public void ResumeButton()
    {
        cameraController.EnableInput();
        zoomController.EnableInput();

        UIScreen.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false); 

        Camera.main.GetComponent<AudioSource>().Play();
    }

    public void QuitGame()
    {
        Debug.Log("Application Closed"); //Just for testing since you need to build the executable for it to work.
        Application.Quit();
    }

    public void AboutGame()
    {
        pauseMenu.gameObject.SetActive(false);
        aboutScreen.gameObject.SetActive(true);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Reloads current scene.
    }

    public void BackButton()
    {
        aboutScreen.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
    }

    public void AddScore()
    {
        //Counter based on required Units for win condition.
        score--;
        audioSource.PlayOneShot(celebration);
    }

    IEnumerator DisplayObjective()
    {
        int counter = 0;
        while(counter != 4) //Arbitrary Timer for flashing text.
        {
            objectiveText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            objectiveText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);

            counter++;
        }
        
        nameText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        scoreObject.gameObject.SetActive(true);
    }

    IEnumerator WinScreen()
    {
        if(score == 0)
        {
            int counter = 0;
            while(counter != 2) //Timer set to the length of the music.
            {
                winScreen.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                winScreen.gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);

                counter++;
            }
            
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //Load scene previous based on the build order.
        }
    }
}
