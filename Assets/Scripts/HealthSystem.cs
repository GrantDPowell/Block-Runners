using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private Player player;

    void Start()
    {
        currentHealth = maxHealth;
        player = GetComponent<Player>(); // Get reference to the Player script
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    void GameOver()
    {
        // Save high score if the current score is higher
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (player.Score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", player.Score);
        }

        // Save top speed if the current top speed is higher
        float currentTopSpeed = PlayerPrefs.GetFloat("TopSpeed", 0);
        if (player.TopSpeed > currentTopSpeed)
        {
            PlayerPrefs.SetFloat("TopSpeed", player.TopSpeed);
        }

        // Save max combo if the current max combo is higher
        int currentMaxCombo = PlayerPrefs.GetInt("MaxCombo", 0);
        if (player.MaxCombo > currentMaxCombo)
        {
            PlayerPrefs.SetInt("MaxCombo", player.MaxCombo);
        }

        PlayerPrefs.Save();

        // Load game over scene
        SceneManager.LoadScene("GameOver");
    }
}
