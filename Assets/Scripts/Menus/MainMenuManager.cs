using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pearlsText;

    public GameObject canvas;
    public GameObject[] panels;
    private ColorList colorList;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        colorList = GetComponentInChildren<ColorList>();
        SetColor(colorList.colors.First(i => i.key == PlayerPrefs.GetString("UIColor", "Green")).key);

        scoreText.text = UserDataManager.GetHighScore().ToString();
        StartCoroutine(DisplayUserDataValues());
    }

    public IEnumerator DisplayUserDataValues()
    {
        yield return new WaitUntil(() => UserDataManager.DataLoaded);
        pearlsText.text = UserDataManager.GetTotalPearls().ToString();
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
