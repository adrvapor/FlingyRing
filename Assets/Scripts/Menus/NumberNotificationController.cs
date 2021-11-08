using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberNotificationController : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Number;

    public void SetNotification(int number)
    {
        if(number <= 0)
        {
            Icon.enabled = false;
            Number.text = "";
        }
        else
        {
            Icon.enabled = true;
            Number.text = number.ToString();
        }
    }

    public void DeactivateNotification() => SetNotification(0);
}
