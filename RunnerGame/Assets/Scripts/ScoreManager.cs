using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Linq;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] HighscoreTable highscoreTable;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    int score = 0;
    int bestScore;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject highscoreList;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        bestScore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = "Score: " + score.ToString();
        bestScoreText.text = "Best Score: " + bestScore.ToString();
    }

    public void AddPoints(int numberOfPoints)
    {
        score += numberOfPoints;
        scoreText.text = "Score: " + score.ToString();

        if (bestScore < score)
        {
            bestScore = score;
            bestScoreText.text = "Best Score: " + bestScore.ToString();
            PlayerPrefs.SetInt("highscore", bestScore);
        }
    }

    public void ViewHighscoreList()
    {
        gameOverScreen.SetActive(false);
        highscoreList.SetActive(true);
    }


    public void BackToGameOverScreen()
    {
        highscoreList.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void AddResultToHighscore(GameObject playerNameFromInput)
    {
        var textScore = scoreText.GetComponent<TextMeshProUGUI>().text;
        var scoreString = textScore.Split(char.Parse(" "));

        UnityEngine.Debug.Log(Int32.Parse(scoreString[1]));
        UnityEngine.Debug.Log(playerNameFromInput.GetComponent<TextMeshProUGUI>().text);

        highscoreTable.AddHighscoreEntry(Int32.Parse(scoreString[1]), playerNameFromInput.GetComponent<TextMeshProUGUI>().text);
    }

    public void InputHighScore(string playerNameFromInput)
    {
        string playerName = playerNameFromInput;
        int currentScore = score;
        UnityEngine.Debug.Log("Received UserName: " + playerNameFromInput);

        List<HighScore> highscores = new List<HighScore>();

        HighScore highscore = new HighScore(playerName, currentScore);
        UnityEngine.Debug.Log("Created new highscore with: name " + highscore.Name + " and score " + highscore.Score);

        PlayerPrefs.SetString("Name", highscore.Name);
        PlayerPrefs.SetInt("Score for " + highscore.Name, highscore.Score);
    }
}
