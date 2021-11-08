using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string LastDayPlayed;
    public int TotalPearls;
    public int TotalLives;
    public List<string> UnlockedRings;

    public UserData()
    {
        LastDayPlayed = new DateTime(1999, 12, 8).ToString();
        TotalPearls = 0;
        TotalLives = 0;
        UnlockedRings = new List<string>(){ "Default" };
    }

    public void SetLastDayPlayed(DateTime dateTime)
    {
        LastDayPlayed = dateTime.ToString();
    }

    public void SetLastDayPlayedToToday()
    {
        LastDayPlayed = DateTime.Now.ToString();
    }

    public DateTime GetLastDayPlayed()
    {
        return DateTime.Parse(LastDayPlayed);
    }
}
