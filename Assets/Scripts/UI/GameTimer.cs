using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private float timeRemaining;
    private bool timerRunning = true;

    public static GameTimer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timeRemaining = timeLimit;
    }

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                GameOver();
            }
        }
    }
    

    void GameOver()
    {
        Debug.Log("Time's up! Game Over!");
        SceneManager.LoadScene("Scenes/GameOverScene");
    }
    
    public void AddTime(float seconds)
    {
        float oldTime = timeRemaining;
        timeRemaining += seconds;
        Debug.Log($"Timer BEFORE: {oldTime:F2} | Added: {seconds} | Timer AFTER: {timeRemaining:F2}");
    }
}