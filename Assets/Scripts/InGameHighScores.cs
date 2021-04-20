﻿using System;
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
        RetrieveHighScores();

        SortHighScores();
        highscore.text = "HIGHSCORE: " + currentHighscore;

    }

   
    private void RetrieveHighScores()
    {
        using (TextReader file = File.OpenText(highscoreFile))
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

        currentLowestscore = allScores[allScores.Count - 1].score;
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

    public void TryToAddScore()
    {
        int currentPlayerScore = playerObject.getScore();

       if (currentPlayerScore > currentHighscore)
        {
            Debug.Log("YOu beat the highscore!");
            //different popup or something ingame
        }
       //ENTRY NOT NEEDED BUT WILL USE FOR NOW
        HighScoreEntry entry;
        entry.name = "George"; //get user input
        entry.score = currentPlayerScore;
        string line = entry.name + " " + entry.score;
        File.AppendAllText( highscoreFile, Environment.NewLine + line);
    }

   
}
