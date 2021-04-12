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

    GameObject[] collectables;

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

    [SerializeField]
    GameObject pausedUI;

    [SerializeField]
    GameObject Red, Orange, Cyan, Pink;


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
        collectables = FindGameObjectsWithTags(new string[] { "Pellet", "Powerup" });
        StartGame();
    }

    void StartGame()
    {
        Time.timeScale = 1;
        gameMode = InGameMode.InGame;
        gameUI.gameObject.SetActive(true);
        winUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerObject.isActiveAndEnabled && playerObject.getLives() != 0)
        {
            newLife();
        }
        if (playerObject.PelletsEaten() == collectables.Length)
        {
            Time.timeScale = 0;
            StartWin();
        }
        else if (playerObject.getLives() == 0)
        {
            Time.timeScale = 0;
            StartLose();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartPause();
        }
        switch (gameMode)
        {

            case InGameMode.InGame:       UpdateMainGame(); break;
            case InGameMode.Win:          UpdateEnd(); break;
            case InGameMode.Lose:         UpdateEnd(); break;
            case InGameMode.Paused:       UpdatePause(); break;  

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

    void UpdatePause()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pausedUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            playerObject.setSpeed(0.02f);
            gameMode = InGameMode.InGame;
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("MainMenu");
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

    void StartPause()
    {
        Time.timeScale = 0;
        playerObject.setSpeed(0);
        gameMode = InGameMode.Paused;
        pausedUI.gameObject.SetActive(true);
    }

    void newLife()
    {
        Red.GetComponent<GhostMovement>().toSpawn();
        Orange.GetComponent<GhostMovement>().toSpawn();
        Pink.GetComponent<GhostMovement>().toSpawn();
        Cyan.GetComponent<GhostMovement>().toSpawn();
        playerObject.gameObject.SetActive(true);
    }
}
