using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip menuSelection;

    public GameObject mainMenu;
    public GameObject highScores;
    public GameObject[] levels;
    public GameObject popup;
    public InputField nameInput;
    public Text errorMessage;
    public Animator transition;

    int currentLevelShown = 0;
    int numberOfLevels;

    AudioSource audioSource;
            
    void Start()
    {
        numberOfLevels = levels.Length;
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
        StartMainMenu();
    }

    // when button is pressed, checks input box for valid name and if so, game is started, otherwise error message appears
    public void PlayGameButton()
    {
        if (CheckValidName())
        {
            audioSource.PlayOneShot(menuSelection);
            transition.SetTrigger("Start");
            Invoke(nameof(StartGame), 1);
        }
        else
        {
            popup.SetActive(true);
        }
    }

    // checks player name given is between 3 and 15 characters and has no spaces. If it has an error, popup error text is set and returns false
    private bool CheckValidName()
    {
        string playerName = nameInput.text;

        if (playerName.Length < 3 || playerName.Length > 15)
        {
            errorMessage.text = "Name must be between 3 and 15 characters";
        }
        else if (playerName.Contains(" "))
        {
            errorMessage.text = "Name must not contain spaces";
        }
        else
        {
            PlayerPrefs.SetString("player", playerName);
            return true;
        }
        return false;
    }
    // first level scene is loaded
    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // moves player from high scores to menu, resetting the error message at same time
    public void MainMenuButton()
    {
        audioSource.PlayOneShot(menuSelection);
        StartMainMenu();
    }
    public void StartMainMenu()
    {
        errorMessage.text = "";
        mainMenu.SetActive(true);
        highScores.SetActive(false);
        popup.SetActive(false);
    }

    // moves player from main menu to the highscores
    public void HighScoresButton()
    {
        audioSource.PlayOneShot(menuSelection);
        levels[currentLevelShown].SetActive(true);
        mainMenu.SetActive(false);
        highScores.SetActive(true);
    }
    
    // cycles right through the level scores
    public void MoveRightThroughScores()
    {
        levels[currentLevelShown].SetActive(false);
        currentLevelShown = (currentLevelShown + 1) % numberOfLevels;
        levels[currentLevelShown].SetActive(true);
    }
    // cycles left through the level scores
    public void MoveLeftThroughScores()
    {
        levels[currentLevelShown].SetActive(false);
        currentLevelShown = (currentLevelShown - 1);
        if(currentLevelShown == -1)
        {
            currentLevelShown = numberOfLevels - 1;
        }
        levels[currentLevelShown].SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
