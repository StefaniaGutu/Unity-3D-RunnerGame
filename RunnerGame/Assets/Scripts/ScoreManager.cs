using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        }
    }
}
