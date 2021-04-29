using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    public string highscoreFile = "level1scores.txt";
    [SerializeField] Font scoreFont;

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

    public void SortHighScoreEntries()
    {
        allScores.Sort((x, y) => y.score.CompareTo(x.score));
    }

    void CreateHighScoreText()
    {
        int scoresToOutput = 10;

        // will output only the top ten scores, but if less than ten, all the scores in file
        if (allScores.Count < 10)
            scoresToOutput = allScores.Count;

        for (int i = 0; i < scoresToOutput; ++i)
        {
            GameObject o = new GameObject();
            o.transform.parent = transform;          

            Text t = o.AddComponent<Text>();
            t.text = allScores[i].name + "\t\t" + allScores[i].score;
            t.font = scoreFont;
            t.fontSize = 20;
            t.color = Color.black;

            o.transform.localPosition = new Vector3(60, -70 + (-(i) * 20), 0);

            o.transform.localRotation = Quaternion.identity;

            o.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 100);
        }

    }

    
}
