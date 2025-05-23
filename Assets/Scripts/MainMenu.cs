
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputField NameInput;
    public Text BestScoreText;
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(GameManager.Instance.BestName))
            BestScoreText.text = $"Best Score: {GameManager.Instance.BestName} : {GameManager.Instance.BestScore}";
        else
            BestScoreText.text = "Best Score: -";
    }
    public void OnStartClicked()
    {
        string name = NameInput.text;
        if (string.IsNullOrWhiteSpace(name)) return;

        GameManager.Instance.SetPlayerName(name);
       SceneManager.LoadScene(1);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
