using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public int TotalPearls;
    public int TotalLives;
    public List<string> UnlockedRings;

    public UserData()
    {
        TotalPearls = 0;
        TotalLives = 0;
        UnlockedRings = new List<string>(){ "Default", "Gear" };
    }
}
