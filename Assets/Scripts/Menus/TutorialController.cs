using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public Button CloseButton;
    public Button PrevButton;
    public Button NextButton;

    public List<GameObject> TutorialScreens;
    public int currentScreen = 0;
    public TextMeshProUGUI currentScreenText;

    public void OpenTutorial()
    {
        currentScreen = 0;
        gameObject.SetActive(true);
        TutorialScreens[currentScreen].SetActive(true);
        UpdateButtonsAndText();
    }

    public void CloseTutorial()
    {
        TutorialScreens[currentScreen].SetActive(false);
        gameObject.SetActive(false);
    }

    public void NextScreen()
    {
        if(currentScreen < TutorialScreens.Count - 1)
        {
            TutorialScreens[currentScreen].SetActive(false);
            currentScreen++;
            TutorialScreens[currentScreen].SetActive(true);

            UpdateButtonsAndText();
        }
    }

    public void PreviousScreen()
    {
        if (currentScreen > 0)
        {
            TutorialScreens[currentScreen].SetActive(false);
            currentScreen--;
            TutorialScreens[currentScreen].SetActive(true);

            UpdateButtonsAndText();
        }
    }

    public void UpdateButtonsAndText()
    {
        currentScreenText.text = $"{currentScreen + 1}/{TutorialScreens.Count}";
        PrevButton.gameObject.SetActive(currentScreen > 0);
        NextButton.gameObject.SetActive(currentScreen < TutorialScreens.Count - 1);
    }
}
