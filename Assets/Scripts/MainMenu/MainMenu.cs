using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioClip menuSelection;

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

    void StartGame()
    {
        Debug.Log("Starting Game!");
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        audioSource.PlayOneShot(menuSelection);
        Invoke(nameof(StartMainMenu), 1);
    }
    public void StartMainMenu()
    {
        errorMessage.text = "";
        mainMenu.SetActive(true);
        highScores.SetActive(false);
        popup.SetActive(false);
    }

    public void HighScoresButton()
    {
        audioSource.PlayOneShot(menuSelection);
        Invoke(nameof(StartHighScores), 1);
    }
    void StartHighScores()
    {
        levels[currentLevelShown].SetActive(true);
        mainMenu.SetActive(false);
        highScores.SetActive(true);
    }

    public void MoveRightThroughScores()
    {
        levels[currentLevelShown].SetActive(false);
        currentLevelShown = (currentLevelShown + 1) % numberOfLevels;
        levels[currentLevelShown].SetActive(true);

    }

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
