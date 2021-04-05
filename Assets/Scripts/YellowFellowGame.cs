using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YellowFellowGame : MonoBehaviour
{
    [SerializeField]
    Fellow playerObject;

    GameObject[] pellets;

    [SerializeField]
    GameObject gameUI;

    [SerializeField]
    GameObject Score;

    Text scoreText;

    [SerializeField]
    GameObject Lives;

    Text livesText;

    [SerializeField]
    GameObject Level;

    Text levelText;

    [SerializeField]
    GameObject winUI;

    [SerializeField]
    GameObject loseUI;

    //function got from this link: https://answers.unity.com/questions/973677/add-gameobjects-with-different-tags-to-one-array.html on 26/03/2021
    GameObject[] FindGameObjectsWithTags(string[] tags)
    {
        var all = new List<GameObject>();

        foreach (string tag in tags)
        {
            var temp = GameObject.FindGameObjectsWithTag(tag).ToList();
            all = all.Concat(temp).ToList();

        }
        return all.ToArray();
    }

    enum InGameMode
    {
        InGame,
        Paused,
        Win,
        Lose
    }

    InGameMode gameMode = InGameMode.InGame;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = Score.GetComponent<Text>();
        livesText = Lives.GetComponent<Text>();
        levelText = Level.GetComponent<Text>();
        pellets = FindGameObjectsWithTags(new string[] { "Pellet", "Powerup" });
        StartGame();
    }

    
    void StartGame()
    {
        gameMode = InGameMode.InGame;
        gameUI.gameObject.SetActive(true);
        winUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject.PelletsEaten() == pellets.Length)
        {
            StartWin();
        }
        else if (playerObject.getLives() == 0)
        {
            StartLose();
        }
        switch (gameMode)
        {

            case InGameMode.InGame:       UpdateMainGame(); break;
            case InGameMode.Win:          UpdateEnd(); break;
            case InGameMode.Lose:         UpdateEnd(); break;

        }
    }
   

    void UpdateMainGame()
    {
        
        scoreText.text = "SCORE: " + playerObject.getScore();
        livesText.text = "LIVES: " + playerObject.getLives();
        //levelText.text = "LEVEL: 1";
    }

    void UpdateEnd()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Level1");
        }
    }

    void StartWin()
    {
        gameMode = InGameMode.Win;
        winUI.gameObject.SetActive(true);
        
    }

    void StartLose()
    {
        gameMode = InGameMode.Lose;
        loseUI.gameObject.SetActive(true);
       // winUI.gameObject.SetActive(true);
    }

}
