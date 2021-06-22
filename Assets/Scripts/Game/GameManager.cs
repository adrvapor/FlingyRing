using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public float score = 0;
    public int pearls = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pearlText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalPearlText;
    public GameObject pauseButton;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public GameObject tooltip;

    public GameObject panel;

    private PlayerController player;

    void Awake()
    {
        instance = this;
        score = 0;
        pearls = 0;
    }

    void Start()
    {
        panel.GetComponent<Image>().color = GetComponentInChildren<ColorList>().colors
            .First(i => i.key == PlayerPrefs.GetString("UIColor")).color;

        ContinueGame();
    }

    public static void UpdateScore(float position)
    {
        if (position - instance.score is float diff && diff > 0)
        {
            instance.score += diff;
            instance.scoreText.text = ((int)instance.score).ToString() + " m";
        }
    }

    public static void AddPearl(int pearls = 1)
    {
        instance.pearls += pearls;
        instance.pearlText.text = instance.pearls.ToString();
    }

    #region Game Over
    public static void GameOver()
    {
        Time.timeScale = 0;
        instance.finalScoreText.text = ((int)instance.score).ToString();
        instance.finalPearlText.text = instance.pearls.ToString();

        instance.gameOverMenu.SetActive(true);
        instance.pauseButton.SetActive(false);
        instance.tooltip.SetActive(false);
    }

    public static void ContinueWithHeart()
    {
        instance.gameOverMenu.SetActive(false);
        instance.pauseButton.SetActive(true);
        instance.tooltip.SetActive(true);
        instance.player.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
    #endregion

    #region Pause
    public static void PauseGame()
    {
        Time.timeScale = 0;
        instance.pauseMenu.SetActive(true);
        instance.pauseButton.SetActive(false);
        instance.tooltip.SetActive(false);
    }

    public static void ContinueGame()
    {
        instance.pauseMenu.SetActive(false);
        instance.pauseButton.SetActive(true);
        instance.tooltip.SetActive(true);
        Time.timeScale = 1;
    }

    public static void GoToSettingsFromPause()
    {
        instance.pauseMenu.SetActive(false);
        instance.settingsMenu.SetActive(true);
    }

    public static void GoToPauseFromSettings()
    {
        instance.pauseMenu.SetActive(true);
        instance.settingsMenu.SetActive(false);
    }

    #endregion

    #region Common options
    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void GoToMainMenu()
    {
        // añadir puntuación a leaderboards y toda la pesca
        UserData.UpdateScore((int)instance.score);
        UserData.UpdatePearls(instance.pearls);

        SceneManager.LoadScene("MenuScene");
    }
    #endregion

    public static void SetPlayer(PlayerController player)
    {
        instance.player = player;
    }

    public static float GetScore() { return instance.score; }
}
