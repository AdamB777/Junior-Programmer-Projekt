using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    // Brick setup
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    // UI components
    public Text ScoreText;       // shows current score
    public Text LivesText;       // shows remaining lives
    public Text NameText;        // shows player name
    public Text BestScoreText;   // shows best score + name
    public GameObject GameOverText;    // game over UI

    private bool m_Started = false;
    private bool m_GameOver = false;
    private int m_Points = 0;
    private int m_Lives = 3;

    void Start()
    {
        // Initialize UI with saved data
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        string bestName = PlayerPrefs.GetString("BestName", "");
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        NameText.text = $"Player: {playerName}";
        BestScoreText.text = $"Best Score: {bestName} : {bestScore}";
        LivesText.text = $"Lives: {m_Lives}";
        ScoreText.text = $"Score : {m_Points}";

        // Create bricks
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void Update()
    {
        if (!m_Started)
        {
            // Press Space to launch the ball
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0).normalized;
                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            // Press Space to go back to Menu (scene index 0)
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(0);
        }
    }

    // Called by each Brick when destroyed
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    // Call this from your DeathZone script when the ball falls
    public void BallLost()
    {
        m_Lives--;
        LivesText.text = $"Lives: {m_Lives}";

        if (m_Lives <= 0)
        {
            EndGame();
        }
        else
        {
            // Reset ball position for next life
            m_Started = false;
            Ball.transform.SetParent(transform);
            Ball.transform.localPosition = Vector3.zero;
            Ball.velocity = Vector3.zero;
            Ball.angularVelocity = Vector3.zero;
        }
    }

    // Handle game over: show UI and update record if needed
    private void EndGame()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        int currentBest = PlayerPrefs.GetInt("BestScore", 0);
        if (m_Points > currentBest)
        {
            string playerName = PlayerPrefs.GetString("PlayerName", "Player");
            PlayerPrefs.SetInt("BestScore", m_Points);
            PlayerPrefs.SetString("BestName", playerName);
            PlayerPrefs.Save();

            BestScoreText.text = $"Best Score: {playerName} : {m_Points}";
        }
    }
}
