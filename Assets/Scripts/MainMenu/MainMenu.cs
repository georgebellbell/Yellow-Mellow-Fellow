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
    public GameObject popup;
    public InputField nameInput;
    public Text errorMessage;

    AudioSource audioSource;
            
    void Start()
    {
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
        StartMainMenu();
    }

   
    public void StartPopupButton()
    {
        audioSource.PlayOneShot(menuSelection);
        popup.SetActive(true);
    }

    public void PlayGameButton()
    {
        if (CheckValidName())
        {
            audioSource.PlayOneShot(menuSelection);
            Invoke(nameof(StartGame), 1);
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
        Debug.Log("Starting Scores!");
        mainMenu.SetActive(false);
        highScores.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
