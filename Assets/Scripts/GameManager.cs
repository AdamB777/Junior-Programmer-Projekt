using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ✨ Singleton – dostęp z każdego miejsca jako GameManager.Instance
    public static GameManager Instance { get; private set; }

    // Dane gracza
    public string PlayerName { get; private set; }
    public int    Score      { get; private set; }
    public int    Lives      { get; private set; }

    // Rekord
    public string BestName  { get; private set; }
    public int    BestScore { get; private set; }

    private void Awake()
    {
        // Jeśli nie ma instancji – ustaw i nie niszcz przy zmianie sceny
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();  // wczytaj z PlayerPrefs
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        BestName  = PlayerPrefs.GetString("BestName", "");
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetString("BestName",  BestName);
        PlayerPrefs.SetInt   ("BestScore", BestScore);
        PlayerPrefs.Save();
    }

    // Ustaw imię przed startem gry
    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    // Zainicjuj nową rozgrywkę
    public void StartNewGame()
    {
        Score = 0;
        Lives = 3;
        SceneManager.LoadScene("GameScene");
    }

    // Dodaj punkty
    public void AddScore(int points)
    {
        Score += points;
    }

    // Odejmij życie
    public void LoseLife()
    {
        Lives--;
        if (Lives <= 0)
            EndGame();
    }

    // Kończy rozgrywkę, zapisuje rekord jeśli jest lepszy, wraca do menu
    private void EndGame()
    {
        if (Score > BestScore)
        {
            BestScore = Score;
            BestName  = PlayerName;
            SaveData();
        }
        SceneManager.LoadScene("MenuScene");
    }
}
