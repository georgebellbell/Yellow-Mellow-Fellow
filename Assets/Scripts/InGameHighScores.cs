using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InGameHighScores : MonoBehaviour
{
    [SerializeField] string highscoreFile = "scores.txt";
    [SerializeField] Text highscore, score;
    [SerializeField] Fellow playerObject;


    int currentHighscore;
    int currentLowestscore;
    bool newHighscoreAchieved = false;
    struct HighScoreEntry
    {
        public int score;
        public string name;
    }

    List<HighScoreEntry> allScores = new List<HighScoreEntry>();
    // Start is called before the first frame update
    void Start()
    {
        using (TextReader file = File.OpenText(highscoreFile))
        {
            string text = null;
            while ((text = file.ReadLine()) != null)
            {
                Debug.Log(text);
                string[] splits = text.Split(' ');
                HighScoreEntry entry;
                entry.name = splits[0];
                entry.score = int.Parse(splits[1]);
                allScores.Add(entry);
            }
        }

        allScores.Sort((x, y) => y.score.CompareTo(x.score));
        currentHighscore = allScores[0].score;
        //currentLowestscore = allScores[allScores.Count].score;
        highscore.text = "HIGHSCORE: " + currentHighscore;

    } 

    // Update is called once per frame
    void Update()
    {
        int currentPlayerScore = playerObject.getScore();

        score.text = "SCORE: " + currentPlayerScore;
        if (currentPlayerScore > currentHighscore)
        {
            highscore.text = "HIGHSCORE: " + currentPlayerScore;
        }
    }

    public void addScore()
    {
        int currentPlayerScore = playerObject.getScore();
        if (cu)
    }


}
