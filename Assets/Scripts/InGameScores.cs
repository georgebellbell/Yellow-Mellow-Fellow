using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InGameScores : MonoBehaviour
{
    public string highscoreFile = "level1scores.txt";
    public Text highscore, score;
    public FellowInteractions playerObject;

    int currentHighscore = 0000;
    int currentPlayerScore = 0000;
 
    struct HighScoreEntry
    {
        public int score;
        public string name;
    }

    List<HighScoreEntry> allScores = new List<HighScoreEntry>();

    void Start()
    {
        RetrieveTopScore();
        highscore.text = "" + currentHighscore;
    }

    // respective score file is found, sorted and top score retrieved
    void RetrieveTopScore()
    {
        using (TextReader file = File.OpenText("Highscores/" + highscoreFile))
        {
            string text = null;
            while ((text = file.ReadLine()) != null)
            {
                string[] splits = text.Split(' ');
                HighScoreEntry entry;
                entry.name = splits[0];
                entry.score = int.Parse(splits[1]);
                allScores.Add(entry);
            }
        }

        if (allScores.Count == 0)
            return;

        allScores.Sort((x, y) => y.score.CompareTo(x.score));
        currentHighscore = allScores[0].score;
    }

    void Update()
    {
        currentPlayerScore = playerObject.GetScore();

        score.text = "" + currentPlayerScore;
        // if player score exceeds current highscore, as game progresses both score values in UI will increase
        if (currentPlayerScore > currentHighscore)
        {
            highscore.text = "" + currentPlayerScore;
        }
    }

    // Called by YellowFellowGame when player beats a level
    public void AddPlayerScore()
    {
        currentPlayerScore = playerObject.GetScore();

        string playerName = PlayerPrefs.GetString("player");
        string line = playerName + " " + currentPlayerScore;
        File.AppendAllText("Highscores/" + highscoreFile, line + Environment.NewLine);

    }
}
