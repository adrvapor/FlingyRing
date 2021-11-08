using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI PearlsText;
    public TextMeshProUGUI LivesText;
    public GameObject DailyLifePanel;

    public GameObject[] panels;
    private ColorList colorList;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        colorList = GetComponentInChildren<ColorList>();
        SetColor(colorList.colors.First(i => i.key == PlayerPrefs.GetString("UIColor", "Green")).key);

        ScoreText.text = UserDataManager.GetHighScore().ToString()+"m";
        RefreshUserDataValues();
    }

    public void RefreshUserDataValues()
    {
        StartCoroutine(DisplayUserDataValues());
    }

    public IEnumerator DisplayUserDataValues()
    {
        yield return new WaitUntil(() => UserDataManager.DataLoaded);

        var lastDate = UserDataManager.GetLastDayPlayed();

        PearlsText.text = UserDataManager.GetTotalPearls().ToString();
        LivesText.text = UserDataManager.GetTotalLives().ToString();

        if (lastDate == null || lastDate.Date < DateTime.Now.Date)
        {
            UserDataManager.UpdateLastDayPlayed();
            UserDataManager.UpdateLives(1);
            yield return new WaitForSeconds(1);

            DailyLifePanel.SetActive(true);
            PearlsText.text = UserDataManager.GetTotalPearls().ToString();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SetColor(string key)
    {
        PlayerPrefs.SetString("UIColor", key);
        ColorPanels(colorList.colors.First(i => i.key == key).color);
    }
    public void SetColor(int index)
    {
        PlayerPrefs.SetString("UIColor", colorList.colors[index].key);
        ColorPanels(colorList.colors[index].color);
    }

    public void ColorPanels(Color color)
    {
        foreach(var panel in panels)
        {
            panel.GetComponent<Image>().color = color;
        }
    }
}
