using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Text highScoreText;
    public Text topSpeedText;
    public Text maxComboText;
    public Text scoreText;
    public AudioClip gameOverClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySound(gameOverClip); // Play game over sound

        // Retrieve high score, top speed, and max combo from PlayerPrefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float topSpeed = PlayerPrefs.GetFloat("TopSpeed", 0);
        int maxCombo = PlayerPrefs.GetInt("MaxCombo", 0);
        int score = PlayerPrefs.GetInt("Score", 0);

        // Display high score, top speed, and max combo
        highScoreText.text = "High Score: " + highScore;
        topSpeedText.text = "Top Speed: " + Mathf.RoundToInt(topSpeed).ToString();
        maxComboText.text = "Max Combo: " + maxCombo;
        scoreText.text = "Score: " + score;
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
