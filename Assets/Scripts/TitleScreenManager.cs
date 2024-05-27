using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public Text highScoreText;
    public Text topSpeedText;
    public Text maxComboText;

    void Start()
    {
        // Retrieve high score, top speed, and max combo from PlayerPrefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float topSpeed = PlayerPrefs.GetFloat("TopSpeed", 0);
        int maxCombo = PlayerPrefs.GetInt("MaxCombo", 0);

        // Display high score, top speed, and max combo
        highScoreText.text = "High Score: " + highScore;
        topSpeedText.text = "Top Speed: " + Mathf.RoundToInt(topSpeed).ToString();
        maxComboText.text = "Max Combo: " + maxCombo;
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Ensure the game stops playing in the editor
        #endif
    }
}
