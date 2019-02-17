using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Lives Player 1
    public GameObject player1Lives;
    public Image player1CurrentLives; //image that represents the current number of lives for P1
    public Sprite[] player1LivesCount; //array containing all possible states for P1

    //Lives Player 2
    public GameObject player2Lives;
    public Image player2CurrentLives; //image that represents the current number of lives for P2
    public Sprite[] player2LivesCount; //array containing all possible states for P2


    //Score
    public int score = 0;
    public Text scoreText;
    public int bestScore = 0;
    public Text bestScoreText;

    //Title Panel
    public GameObject title_Panel;

    //Game On Panel
    public GameObject gameOn_Panel;

    //Pause Menu Panel
    public GameObject pauseMenu_Panel;


    //Update Image that represents Player's lives for P1
    public void UpdatePlayer1Lives(int livesCount)
    {
        player1CurrentLives.sprite = this.player1LivesCount[livesCount];
    }

    //Update Image that represents Player's lives for P2
    public void UpdatePlayer2Lives(int livesCount)
    {
        player2CurrentLives.sprite = this.player2LivesCount[livesCount];
    }


    //Update Game score
    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    //Check for Best Score
    public void CheckBestScore()
    {
        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            bestScoreText.text = "Best Score: " + bestScore;
        }
    }

    //Show Title Screen after 3 secs, so we can wait for the explosion animations to be finished
    public void ShowTitleScreen()
    {
        title_Panel.SetActive(true);
        gameOn_Panel.SetActive(false);

    }
    //Hide Title Screen
    public void HideTitleScreen()
    {
        title_Panel.SetActive(false);
        gameOn_Panel.SetActive(true);
    }

    //Enable Pause Menu
    public void EnablePauseMenu()
    {
        pauseMenu_Panel.SetActive(true);
    }

    //Disable Pause Menu
    public void DisablePauseMenu()
    {
        pauseMenu_Panel.SetActive(false);
    }
}
