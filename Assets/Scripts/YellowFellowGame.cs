﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YellowFellowGame : MonoBehaviour
{
    [SerializeField] Fellow playerObject;
    [SerializeField] Text lives, level;
    [SerializeField] GameObject gameUI, winUI, loseUI, pausedUI;
    [SerializeField] AudioClip victory;

    InGameHighScores scores;
    AudioSource audioSource;
    Ghost Red, Orange, Cyan, Pink;

    GameObject[] collectables;
    bool playerScoreAdded = false;
    int currentLevel;
    enum InGameMode
    {
        InGame,
        Paused,
        Win,
        Lose
    }

    InGameMode gameMode = InGameMode.InGame;

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        scores = GetComponent<InGameHighScores>();
        audioSource = GetComponent<AudioSource>();
        SetGhostPaths();
        level.text = "LEVEL: " + currentLevel;
        collectables = FindGameObjectsWithTags(new string[] { "Pellet", "Powerup" });
        StartGame();
    }

    void SetGhostPaths()
    {
        Red = GameObject.Find("red").GetComponent<Ghost>();
        Red.waypoints = assignPath(Red.name);
        Orange = GameObject.Find("orange").GetComponent<Ghost>();
        Orange.waypoints = assignPath(Orange.name);
        Cyan = GameObject.Find("cyan").GetComponent<Ghost>();
        Cyan.waypoints = assignPath(Cyan.name);
        Pink = GameObject.Find("pink").GetComponent<Ghost>();
        Pink.waypoints = assignPath(Pink.name);
    }

    Transform[] assignPath(string name)
    {
        List<Transform> allNodes = new List<Transform>();
        string pathParent = name + "Path";
        GameObject pathObject = GameObject.Find(pathParent);
        foreach (Transform node in pathObject.transform)
        {
            if (node.CompareTag("waypoint"))
            {
                allNodes.Add(node);
            }
        }
        return allNodes.ToArray();
    }

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

    void StartGame()
    {
        playerObject.Resume();
        Time.timeScale = 1;
        gameMode = InGameMode.InGame;

        gameUI.gameObject.SetActive(true);
        winUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
        pausedUI.gameObject.SetActive(false);    
    }

    void Update()
    {
        lives.text = "LIVES: " + playerObject.getLives();

        if (!playerObject.isActiveAndEnabled && playerObject.getLives() > 0)
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
        
    }

    void newLife()
    {
        Red.toSpawn();
        Orange.toSpawn();
        Pink.toSpawn();
        Cyan.toSpawn();
        playerObject.respawn();
    }

    void StartWin()
    {
        if (!playerScoreAdded)
        {
            scores.TryToAddScore();
            playerScoreAdded = true;
        }

        audioSource.PlayOneShot(victory);
        gameMode = InGameMode.Win;
        winUI.gameObject.SetActive(true);
    }

    void StartLose()
    {
        
        gameMode = InGameMode.Lose;
        loseUI.gameObject.SetActive(true);
    }

    void StartPause()
    {
        Time.timeScale = 0;
        playerObject.Pause();
        gameMode = InGameMode.Paused;
        pausedUI.gameObject.SetActive(true);
    }


    public void ResumeGameButton()
    {
        StartGame();
    }

    public void RestartLevelButton()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void StartNextLevelButton()
    {
        int nextLevel = (currentLevel + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextLevel);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene(0);
    }

}
