using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore
{
    public string Name { get; set; }
    public int Score { get; set; }

    public HighScore(string Name, int Score)
    {
        this.Name = Name;
        this.Score = Score;
    }
}
