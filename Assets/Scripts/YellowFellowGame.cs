using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YellowFellowGame : MonoBehaviour
{
    //Game Objects
    public FellowInteractions playerObject;
    Ghost Red, Orange, Cyan, Pink;
    public GameObject[] collectables;

    //Game UI
    public Text lives, level;
    public GameObject gameUI, winUI, finalWinUI, loseUI, pausedUI, SlowMoUI;
    InGameScores scores;
    int currentLevel;

    public AudioClip victory, lose, allLevelsComplete;
    AudioSource audioSource;

    public Animator transition, pausing;

    public GameObject birdsEyeCamera;
    public GameObject closeUpCamera;
   
    public bool paused = false;
    bool gameEnded = false;
    bool finalLevel = false;
   
    void Start()
    {
        scores = GetComponent<InGameScores>();
        audioSource = GetComponent<AudioSource>();

        currentLevel = SceneManager.GetActiveScene().buildIndex;
        level.text = "LEVEL: " + currentLevel;
        //determine if current level is last one
        finalLevel = (currentLevel == SceneManager.sceneCountInBuildSettings - 1);

        birdsEyeCamera.SetActive(true);
        closeUpCamera.SetActive(false);
       
        //All collectables are found
        collectables = FindGameObjectsWithTags(new string[] { "Pellet", "Powerup", "Timeslow", "DoubleScore", "ExtraLife" });
        SetGhostPaths();
        StartGame();
    }

    //finds all objects with the specified tags
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
    
    // Called when game first starts or when unpaused
    void StartGame()
    {
        paused = false;

        gameUI.gameObject.SetActive(true);
        winUI.gameObject.SetActive(false);
        finalWinUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
        pausedUI.gameObject.SetActive(false);    
    }

    void Update()
    {
        // If game has ended, don't do anything else
        if (gameEnded)
            return;

        lives.text = "" + playerObject.GetLives();

        // if player is dead, and has lives left, respawn player and reset ghosts
        if (!playerObject.isActiveAndEnabled && playerObject.GetLives() > 0)
        {
            ResetGame();
        }
        
        // if all the collectables have been eaten, the player wins
        if (playerObject.PelletsEaten() == collectables.Length)
        {
            GameEnd();

            scores.AddPlayerScore();   
           
            if (finalLevel)
                StartFinalWin();
            else
                StartWin();
        }
        // if player has no lives left, game ends
        else if (playerObject.GetLives() == 0)
        {
            GameEnd();
            StartLose();
        }

        //if the Timeslow powerup is active, grey overlay will appear
        SlowMoUI.SetActive(playerObject.IsTimeslowActive());

        //pausing game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            if (paused)
                StartPause(); 
            else
                StartGame(); 
        }
    }

    //resets both player and ghosts back to spawn point
    void ResetGame()
    {
        Red.toSpawn();
        Orange.toSpawn();
        Pink.toSpawn();
        Cyan.toSpawn();
        playerObject.respawn();
    }
    
    //when game has ended, nothing further will happen in game
    void GameEnd()
    {
        paused = true;
        gameEnded = true;
    }

    // if last level when game won, final win UI appears
    void StartFinalWin()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(allLevelsComplete);

        finalWinUI.gameObject.SetActive(true);
    }

    // otherwise the normal win UI will appear
    void StartWin()
    {
        if (!audioSource.isPlaying)
                audioSource.PlayOneShot(victory);

        winUI.gameObject.SetActive(true);
    }
   
    // if player loses, lose UI will appear
    void StartLose()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(lose);
        
        loseUI.gameObject.SetActive(true);
    }

    // when escape is pressed, paused Ui will appear through an animation
    void StartPause()
    {
        pausedUI.SetActive(true);
        pausing.SetTrigger("Pause");
    }

    // BUTTONS

    // resumes game from paused menu
    public void ResumeGameButton()
    {
        StartGame();
    }

    // switches camera to follow camera
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

    // reloads the current game level scene
    public void RestartLevelButton()
    {
        StartCoroutine(LoadLevel(currentLevel));
    }

    // loads the main menu scene
    public void QuitButton()
    {
        StartCoroutine(LoadLevel(0));
    }

    // from either win UI, can progress to next level scene
    public void StartNextLevelButton()
    {
        int nextLevel = (currentLevel + 1) % SceneManager.sceneCountInBuildSettings;
        StartCoroutine(LoadLevel(nextLevel));
    }

    // from final win UI, you can restart from level 1 scene
    public void StartLevelOne()
    {
        StartCoroutine(LoadLevel(1));
    }

    // coroutine to start transition animation and load new scene
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);

    }
}
