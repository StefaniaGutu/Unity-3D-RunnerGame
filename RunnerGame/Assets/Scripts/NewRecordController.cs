using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewRecordController : MonoBehaviour
{
    public static NewRecordController instance;

    public TextMeshProUGUI endScoreText;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        endScoreText.text = "";

    }

    public void NewRecord()
    {
        endScoreText.text = "New Highscore!!!";
    }
}
