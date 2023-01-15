using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    public GameObject[] lives;
 
    public int remainingLives;

    public bool isDead;

    public void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        remainingLives = lives.Length;
        isDead = false;
    }

    public void RemoveLife()
    {
        if (remainingLives>=1)
        {
            remainingLives -= 1;
            UnityEngine.Debug.Log("Player lost 1 life");
            Destroy(lives[remainingLives].gameObject);
            if(remainingLives < 1)
                isDead = true;
        }
    }
}
