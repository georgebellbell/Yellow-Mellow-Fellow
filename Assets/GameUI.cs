using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [SerializeField]
    Fellow playerObject;

    [SerializeField]
    GameObject Score;

    Text scoreText;

    [SerializeField]
    GameObject Lives;

    Text livesText;

    [SerializeField]
    GameObject Level;

    Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = Score.GetComponent<Text>();
        livesText = Lives.GetComponent<Text>();
        levelText = Level.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "SCORE: " + playerObject.getScore();
        //TODO 
        livesText.text = "LIVES: " + playerObject.getLives();
        levelText.text = "LEVEL: 1";

    }
}
