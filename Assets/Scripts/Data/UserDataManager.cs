using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    private static string userDataPath;
    public static UserData LocalUserData;

    // Legacy data
    public static string LegacyDataRetrieved = "LegacyDataRetrieved";
    public static string TotalScoreKey = "TotalScore";
    public static string HighScoreKey = "HighScore";

    public static string SelectedRingKey = "SelectedRing";
    public static string TotalPearlsKey = "TotalPearls";
    public static string TotalLivesKey = "TotalLives";

    public static bool DataLoaded;


    private void Start()
    {
        DataLoaded = false;
        userDataPath = $"{Application.persistentDataPath}/userData.json";
        LoadLocalData();
    }

    private void Awake()
    {
        Debug.Log($"{TotalScoreKey}: {PlayerPrefs.GetInt(TotalScoreKey, 0)}");
        Debug.Log($"{HighScoreKey}: {PlayerPrefs.GetInt(HighScoreKey, 0)}");

        Debug.Log($"{TotalPearlsKey}: {PlayerPrefs.GetInt(TotalPearlsKey, 0)}");
        Debug.Log($"{TotalLivesKey}: {PlayerPrefs.GetInt(TotalLivesKey, 0)}");
    }

    public static void LoadLocalData()
    {
        DataLoaded = false;

        if (File.Exists(userDataPath))
        {
            string json = File.ReadAllText(userDataPath);
            LocalUserData = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            var newUserData = new UserData();
            LocalUserData = newUserData;
        }
        LegacyDataRecovery();

        DataLoaded = true;
    }

    public static void SaveDataLocally(UserData userData)
    {
        string json = JsonUtility.ToJson(userData);
        File.WriteAllText(userDataPath, json);
    }

    public static void SaveDataLocally()
    {
        string json = JsonUtility.ToJson(LocalUserData);
        File.WriteAllText(userDataPath, json);
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

    public static void UpdateSelectedRing(string key)
    {
        PlayerPrefs.SetString(SelectedRingKey, key);
    }

    public static void UpdateScore(int score)
    {
        UpdateHighScore(score);
        UpdateTotalScore(score);
    }

    public static void UpdatePearls(int pearls)
    {
        PlayerPrefs.SetInt(TotalPearlsKey, pearls + PlayerPrefs.GetInt(TotalPearlsKey, 0));
        LocalUserData.TotalPearls += pearls;
        SaveDataLocally();
    }

    public static void UpdateLives(int lives)
    {
        PlayerPrefs.SetInt(TotalLivesKey, lives + PlayerPrefs.GetInt(TotalLivesKey, 0));
        LocalUserData.TotalLives += lives;
        SaveDataLocally();
    }
    #endregion

    #region Get values
    public static int GetTotalScore() => PlayerPrefs.GetInt(TotalScoreKey, 0);
    public static int GetHighScore() => PlayerPrefs.GetInt(HighScoreKey, 0);
    public static string GetSelectedRing() => PlayerPrefs.GetString(SelectedRingKey, "Default");
    public static int GetTotalPearls() => LocalUserData.TotalPearls;//PlayerPrefs.GetInt(TotalPearlsKey, 0);
    public static int GetTotalLives() => LocalUserData.TotalLives;//PlayerPrefs.GetInt(TotalLivesKey, 0);
    public static List<string> GetUnlockedRings() => LocalUserData.UnlockedRings;
    #endregion

    private static void LegacyDataRecovery()
    {
        if (!PlayerPrefs.HasKey(LegacyDataRetrieved))
        {
            if (PlayerPrefs.HasKey(TotalPearlsKey))
            {
                LocalUserData.TotalPearls = PlayerPrefs.GetInt(TotalPearlsKey);
            }
            if (PlayerPrefs.HasKey(TotalLivesKey))
            {
                LocalUserData.TotalLives = PlayerPrefs.GetInt(TotalLivesKey);
            }
            SaveDataLocally();
            PlayerPrefs.SetInt(LegacyDataRetrieved, 1);
        }
    }
}