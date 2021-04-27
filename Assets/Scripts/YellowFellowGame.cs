using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YellowFellowGame : MonoBehaviour
{
    //Game Objects
    public Fellow playerObject;
    Ghost Red, Orange, Cyan, Pink;
    public GameObject[] collectables;

    //Game UI
    public Text lives, level;
    public GameObject gameUI, winUI, loseUI, pausedUI;
    InGameScores scores;
    int currentLevel;

    public AudioClip victory, lose, allLevelsComplete;
    AudioSource audioSource;

    public Animator transition, pausing;

    public GameObject birdsEyeCamera;
    public GameObject closeUpCamera;
   
    bool playerScoreAdded = false;
    bool paused = false;
    bool gameEnded = false;
   
    void Start()
    {
        birdsEyeCamera.SetActive(true);
        closeUpCamera.SetActive(false);

        currentLevel = SceneManager.GetActiveScene().buildIndex;
        scores = GetComponent<InGameScores>();
        audioSource = GetComponent<AudioSource>();
        SetGhostPaths();
        level.text = "LEVEL: " + currentLevel;
        collectables = FindGameObjectsWithTags(new string[] { "Pellet", "Powerup", "Timeslow", "DoubleScore", "ExtraLife" });
        StartGame();
    }

    // When level loads, ghosts are found and their patrol paths are set
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

    //finds the specific path for each ghost
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
        paused = false;
        playerObject.Resume();
        SetGhostSpeed(3.5f);

        gameUI.gameObject.SetActive(true);
        winUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
        pausedUI.gameObject.SetActive(false);    
    }

    void Update()
    {
        if (gameEnded)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            
        }
        lives.text = "" + playerObject.GetLives();

        // if player is dead, and has lives left, respawn them and reset ghosts
        if (!playerObject.isActiveAndEnabled && playerObject.GetLives() > 0)
        {
            ResetGame();
        }
        
        // if all the collectables have been eaten, the player wins
        if (playerObject.PelletsEaten() == collectables.Length)
        {
            GameEnd();
            StartWin();
        }
        // if player has no lives left, game ends
        else if (playerObject.GetLives() == 0)
        {
            GameEnd();
            StartLose();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            if (paused)
                StartPause();
            else
                StartGame();
            
        }
        
    }

    void ResetGame()
    {
        Red.toSpawn();
        Orange.toSpawn();
        Pink.toSpawn();
        Cyan.toSpawn();
        playerObject.respawn();
    }

    void GameEnd()
    {
        gameEnded = true;
        SetGhostSpeed(0f);
        playerObject.Pause();
    }

    void StartWin()
    {   
        // if score hasnt been added, add it
        if (!playerScoreAdded)
        {
            scores.TryToAddScore();
            playerScoreAdded = true;
        }
        if (currentLevel == SceneManager.sceneCountInBuildSettings - 1)
        {
            // end game cutscene
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(allLevelsComplete);
        }
        else
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(victory);
        }

        winUI.gameObject.SetActive(true);
    }
   
    void StartLose()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(lose);
        
        loseUI.gameObject.SetActive(true);
    }

    void StartPause()
    {
        playerObject.Pause();
        SetGhostSpeed(0f);
        pausedUI.SetActive(true);
        pausing.SetTrigger("Pause");
    }

    void SetGhostSpeed(float speed)
    {
        Red.SetGhostSpeed(speed);
        Cyan.SetGhostSpeed(speed);
        Orange.SetGhostSpeed(speed);
        Pink.SetGhostSpeed(speed);
    }

    // BUTTONS

    public void ResumeGameButton()
    {
        StartGame();
    }

    public void ChangeCameraButton()
    {
        birdsEyeCamera.SetActive(!birdsEyeCamera.activeSelf);
        closeUpCamera.SetActive(!closeUpCamera.activeSelf);

        Text buttonText = GameObject.Find("CameraChangeText").GetComponent<Text>();

        if (birdsEyeCamera.activeSelf)
            buttonText.text = "Maze Camera";
        else
            buttonText.text = "Player Camera";

    }
    public void RestartLevelButton()
    {
        StartCoroutine(LoadLevel(currentLevel));
    }

    public void QuitButton()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void StartNextLevelButton()
    {
        int nextLevel = (currentLevel + 1) % SceneManager.sceneCountInBuildSettings;
        StartCoroutine(LoadLevel(nextLevel));
    }

   
   
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);

    }




}
