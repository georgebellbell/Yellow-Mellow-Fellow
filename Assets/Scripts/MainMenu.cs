using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject highScoreUI;

    [SerializeField]
    GameObject mainMenuUI;

    AudioSource menuAction;
    enum GameMode
    {
        MainMenu,
        HighScores
    }

   
    GameMode gameMode = GameMode.MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        menuAction = GameObject.Find("Instructions").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameMode)
        {
            case GameMode.MainMenu: UpdateMainMenu(); break;
            case GameMode.HighScores: UpdateHighScores(); break;
           
        }

    }

    void UpdateMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            
            StartHighScores();
        }
    }
    void UpdateHighScores()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartMainMenu();
        }
    }

    void StartMainMenu()
    {
        gameMode = GameMode.MainMenu;
        mainMenuUI.gameObject.SetActive(true);
        highScoreUI.gameObject.SetActive(false);
    }


    void StartHighScores()
    {

        gameMode = GameMode.HighScores;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
        
    }
}
