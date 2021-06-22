using TMPro;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static string TotalScoreKey = "TotalScore";
    public static string HighScoreKey = "HighScore";

    public static string TotalPearlsKey = "TotalPearls";
    public static string TotalLivesKey = "TotalLives";

    private void Awake()
    {
        Debug.Log($"{TotalScoreKey}: {PlayerPrefs.GetInt(TotalScoreKey, 0)}");
        Debug.Log($"{HighScoreKey}: {PlayerPrefs.GetInt(HighScoreKey, 0)}");

        Debug.Log($"{TotalPearlsKey}: {PlayerPrefs.GetInt(TotalPearlsKey, 0)}");
        Debug.Log($"{TotalLivesKey}: {PlayerPrefs.GetInt(TotalLivesKey, 0)}");
    }

    #region Update values
    public static void UpdateTotalScore(int score)
    {
        PlayerPrefs.SetInt(TotalScoreKey, score + PlayerPrefs.GetInt(TotalScoreKey, 0));
        Social.ReportScore(PlayerPrefs.GetInt(TotalScoreKey, 1), GPGSIds.leaderboard_total_score, (bool success) => { if (!success) Debug.LogWarning("TotalScore not updated"); });
    }

    public static void UpdateHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt(HighScoreKey, 0))
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            Social.ReportScore(PlayerPrefs.GetInt(HighScoreKey, 1), GPGSIds.leaderboard_high_score, (bool success) => { if (!success) Debug.LogWarning("HighScore not updated"); });
        }
    }

    public static void UpdateScore(int score)
    {
        UpdateHighScore(score);
        UpdateTotalScore(score);
    }

    public static void UpdatePearls(int pearls)
    {
        PlayerPrefs.SetInt(TotalPearlsKey, pearls + PlayerPrefs.GetInt(TotalPearlsKey, 0));
    }

    public static void UpdateLives(int lives)
    {
        PlayerPrefs.SetInt(TotalLivesKey, lives + PlayerPrefs.GetInt(TotalLivesKey, 0));
    }
    #endregion

    #region Get values
    public static int GetTotalScore() => PlayerPrefs.GetInt(TotalScoreKey, 0);
    public static int GetHighScore() => PlayerPrefs.GetInt(HighScoreKey, 0);
    public static int GetTotalPearls() => PlayerPrefs.GetInt(TotalPearlsKey, 0);
    public static int GetTotalLives() => PlayerPrefs.GetInt(TotalLivesKey, 0);
    #endregion
}