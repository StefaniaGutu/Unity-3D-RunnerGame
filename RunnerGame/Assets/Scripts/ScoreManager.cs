using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    int score;
    int bestScore;

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

            NewRecordController.instance.NewRecord();
        }
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
