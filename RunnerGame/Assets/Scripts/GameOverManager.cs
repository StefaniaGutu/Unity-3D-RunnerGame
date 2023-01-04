using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;

    [SerializeField] PlayerController playerController;

    public void SetGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        //playerController.setSpeedEnd();
        SceneManager.LoadScene(1);
    }
}
