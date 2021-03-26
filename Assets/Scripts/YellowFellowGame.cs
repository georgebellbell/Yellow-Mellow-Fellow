using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YellowFellowGame : MonoBehaviour
{
    [SerializeField]
    Fellow playerObject;

    GameObject[] pellets;

    [SerializeField]
    GameObject highScoreUI;

    [SerializeField]
    GameObject mainMenuUI;

    [SerializeField]
    GameObject gameUI;

    [SerializeField]
    GameObject winUI;


    enum GameMode
    {
        InGame,
        MainMenu,
        HighScores
    }

    GameMode gameMode = GameMode.MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        pellets = FindGameObjectsWithTags(new string[] { "Pellet", "Powerup" });
        StartMainMenu(); 
    }

    //function got from this link: https://answers.unity.com/questions/973677/add-gameobjects-with-different-tags-to-one-array.html on 26/03/2021
    GameObject[] FindGameObjectsWithTags(string[] tags)
    {
        var all = new List<GameObject>();

        foreach(string tag in tags)
        {
            var temp = GameObject.FindGameObjectsWithTag(tag).ToList();
            all = all.Concat(temp).ToList();

        }
        return all.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerObject.PelletsEaten() == pellets.Length)
        {
            Debug.Log("Level Completed!");
        }
        switch(gameMode)
        {
            case GameMode.MainMenu:     UpdateMainMenu(); break;
            case GameMode.HighScores:   UpdateHighScores(); break;
            case GameMode.InGame:       UpdateMainGame(); break;
        }
    }

    void UpdateMainMenu()
    {
        if(Input.GetKeyDown(KeyCode.Space))
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

    void UpdateMainGame()
    {
       // playerObject
    }

    void StartMainMenu()
    {
        gameMode                        = GameMode.MainMenu;
        mainMenuUI.gameObject.SetActive(true);
        highScoreUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
    }


    void StartHighScores()
    {
        gameMode                = GameMode.HighScores;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
    }

    void StartGame()
    {
        gameMode                = GameMode.InGame;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
    }
}
