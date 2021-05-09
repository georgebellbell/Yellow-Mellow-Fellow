using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    public string highscoreFile = "level1scores.txt";
    public Font scoreFont;

    List<HighScoreEntry> allScores = new List<HighScoreEntry>();

    struct HighScoreEntry
    {
        public int score;
        public string name;
    }

    void Start()
    {
        LoadHighScoreTable();
        if (allScores.Count > 0)
        {
            SortHighScoreEntries();
            CreateHighScoreText();
        }
    }

    // reads file, breaking values into name and score as entries to be sorted
    public void LoadHighScoreTable()
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

    // orders all sscores in file in decending order
    public void SortHighScoreEntries()
    {
        allScores.Sort((x, y) => y.score.CompareTo(x.score));
    }

    // will output only the top ten scores, but if less than ten, all the scores in file
    void CreateHighScoreText()
    {
        int scoresToOutput = 10;
        
        if (allScores.Count < 10)
            scoresToOutput = allScores.Count;

        for (int i = 0; i < scoresToOutput; ++i)
        {
            GameObject o = new GameObject();
            o.transform.parent = transform;          

            Text t = o.AddComponent<Text>();
            t.text = allScores[i].name + "\t\t" + allScores[i].score;
            t.alignment = TextAnchor.MiddleCenter;
            t.font = scoreFont;
            t.fontSize = 20;
            t.color = Color.black;

            o.transform.localPosition = new Vector3(0, -40+ (-(i) * 25), 0);

            o.transform.localRotation = Quaternion.identity;

            o.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 100);
        }

    }  
}
