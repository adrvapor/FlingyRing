using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExtensions : MonoBehaviour
{
    public Button button;

    public void PressButton() => button.onClick.Invoke();
}
