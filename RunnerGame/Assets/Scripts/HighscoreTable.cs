using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        //highscoreEntryList = new List<HighscoreEntry>
        //{
        //    new HighscoreEntry{ name = "Stefi", score = 54 },
        //    new HighscoreEntry { name = "Adriana", score = 36 },
        //    new HighscoreEntry { name = "Stefania", score = 20 },
        //    new HighscoreEntry { name = "Luciana", score = 13 },
        //    new HighscoreEntry {name = "Teo", score = 12 }
        //};

        //Highscores highscores1 = new Highscores { highscoreEntryList = highscoreEntryList };
        //string json = JsonUtility.ToJson(highscores1);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();

        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "HighscoreEntryTemplate(Clone)");
        foreach(var obj in objects)
        {
            Destroy(obj);
        }

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscoreEntryList = highscores.highscoreEntryList;

        highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        highscoreEntryTransformList = new List<Transform>();
        for(int i = 0; i< 5; i++)
        {
                CreateHighscoreEntryTransform(highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
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

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (!highscores.highscoreEntryList.Contains(highscoreEntry))
        {
            highscores.highscoreEntryList.Add(highscoreEntry);

            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highscoreTable", json);
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
