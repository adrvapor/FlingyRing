using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGSLeaderboards : MonoBehaviour
{
    public static void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    // No hacer contínuamente, quizá cada X tiempo, cada partida por ejemplo.
    public static void UpdateHighScoreLeaderboard()
    {
        // Si no se ha avanzado en la leaderboard se hace return aquí directamente.
        if (PlayerPrefs.GetInt(UserDataManager.HighScoreKey, 0) == 0)
            return;

        Social.ReportScore(PlayerPrefs.GetInt(UserDataManager.TotalScoreKey, 1), GPGSIds.leaderboard_total_score, (bool success) =>
        {

        });

        Social.ReportScore(PlayerPrefs.GetInt(UserDataManager.HighScoreKey, 1), GPGSIds.leaderboard_high_score, (bool success) =>
        {
            // Si se ha podido actualizar la puntuación en la leaderboard, cargarse el valor de PlayerPrefs
            if (success)
                PlayerPrefs.SetInt(UserDataManager.HighScoreKey, 0);
        });
    }
}
