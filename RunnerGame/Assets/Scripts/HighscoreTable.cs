using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "HighscoreEntryTemplate(Clone)");
        foreach(var obj in objects)
        {
            Destroy(obj);
        }

        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        if (string.IsNullOrEmpty(jsonString))
        {
            highscoreEntryList = new List<HighscoreEntry>();
            Highscores highscores1 = new Highscores { highscoreEntryList = highscoreEntryList };
            string json = JsonUtility.ToJson(highscores1);
            PlayerPrefs.SetString("HighscoreTable", json);
            PlayerPrefs.Save();
        }
        else
        {
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

            highscoreEntryList = highscores.highscoreEntryList;

            highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

            highscoreEntryTransformList = new List<Transform>();
            for (int i = 0; i < Math.Min(highscoreEntryList.Count, 5); i++)
            {
                CreateHighscoreEntryTransform(highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
            }
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 50f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count - 50f);
        entryTransform.gameObject.SetActive(true);

        int position = transformList.Count + 1;
        entryTransform.Find("PositionText").GetComponent<TextMeshProUGUI>().text = position.ToString();

        int score = highscoreEntry.score;
        entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("HighscoreTable");

        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        var isContained = false;

        foreach (var entityHighscore in highscores.highscoreEntryList)
        {
            if (highscoreEntry.score == entityHighscore.score && string.Equals(highscoreEntry.name, entityHighscore.score))
            {
                isContained = true;
                break;
            }
        }

        if (!isContained)
        {
            highscores.highscoreEntryList.Add(highscoreEntry);
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("HighscoreTable", json);
            PlayerPrefs.Save();
        }

    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
