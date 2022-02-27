using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timer;

    public GameObject GameOverScreen;
    public BallController Ball;

    public bool isGameActive;

    public int score;
    private float _timeLeft = 60f;


    public void Start()
    {
        Ball = GameObject.Find("Ball").GetComponent<BallController>();
        isGameActive = true;
    }

    public void Update()
    {
        score = Ball.pointValue;
        UpdateScore();
        if (isGameActive)
        {
            TimerOfGame();
        }
        GameOver();
    }

    void GameOver()
    {
        if (!isGameActive)
        {
            GameOverScreen.SetActive(true);
        }
    }       //  ����� ����


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }               //  ������ ������� ����
    public void ExitToMenu(int index)
    {
        SceneManager.LoadScene(index);
    }       //  ������ ����� � ������� ����


    void UpdateScore()
    {
        
        scoreText.text = "Score:" + score.ToString();
    }               // ������ � ������� �����

    public void TimerOfGame()
    {
        _timeLeft -= Time.deltaTime;
        timer.text = "TIME: " + Mathf.Round(_timeLeft);
        if (_timeLeft < 0)
        {
            isGameActive = false;
        }
    }       // ������ � ������� �������� ������
}
