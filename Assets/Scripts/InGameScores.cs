using System;
using System.Collections;
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
 
    struct HighScoreEntry
    {
        public int score;
        public string name;
    }

    List<HighScoreEntry> allScores = new List<HighScoreEntry>();

    void Start()
    {
        RetrieveHighScores();

        SortHighScores();
        highscore.text = "" + currentHighscore;

    }

   
    private void RetrieveHighScores()
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
    }
    private void SortHighScores()
    {
        if (allScores.Count == 0)
            return;

        allScores.Sort((x, y) => y.score.CompareTo(x.score));
        currentHighscore = allScores[0].score;
    }

    void Update()
    {
        int currentPlayerScore = playerObject.GetScore();

        score.text = "" + currentPlayerScore;
        if (currentPlayerScore > currentHighscore)
        {
            highscore.text = "" + currentPlayerScore;
        }
    }

    public void TryToAddScore()
    {
        int currentPlayerScore = playerObject.GetScore();

        string playerName = PlayerPrefs.GetString("player"); //get user input
        string line = playerName + " " + currentPlayerScore;
        File.AppendAllText("Highscores/" + highscoreFile, line + Environment.NewLine);
    }
}
