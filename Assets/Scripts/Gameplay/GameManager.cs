using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public float score = 0;
    public int pearls = 0;
    public bool continueSpent = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pearlText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalPearlText;
    public TextMeshProUGUI continueUnavailableText;

    public GameObject pauseButton;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public GameObject continueButton;
    public GameObject tooltip;
    
    public GameObject panel;

    private PlayerController player;
    private ObstacleGenerator obstacleGenerator;

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

        if(UserDataManager.GetTotalLives() > 0)
        {
            continueButton.SetActive(true);
            continueUnavailableText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UIText", "NoMoreContinues").Result;
        }
        else
        {
            continueButton.SetActive(false);
            continueUnavailableText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UIText", "NoMoreLives").Result;
        }

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
        instance.tooltip.SetActive(Settings.TooltipsOn);

        UserDataManager.UpdateLives(-1);
        instance.continueButton.SetActive(false);

        instance.obstacleGenerator.ClearObstaclesAtRespawn();
        instance.player.Respawn(instance.obstacleGenerator.GetRespawnPosition());
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
        instance.tooltip.SetActive(Settings.TooltipsOn);
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
        UserDataManager.UpdateScore((int)instance.score);
        UserDataManager.UpdatePearls(instance.pearls);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void GoToMainMenu()
    {
        UserDataManager.UpdateScore((int)instance.score);
        UserDataManager.UpdatePearls(instance.pearls);

        SceneManager.LoadScene("MenuScene");
    }
    #endregion

    public static void SetPlayer(PlayerController player)
    {
        instance.player = player;
    }
    public static void SetObstacleGenerator(ObstacleGenerator obstacleGenerator)
    {
        instance.obstacleGenerator = obstacleGenerator;
    }

    public static float GetScore() { return instance.score; }
}
