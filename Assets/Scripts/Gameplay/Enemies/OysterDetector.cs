using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OysterDetector : MonoBehaviour
{
    public bool Closed = false;
    public Collider Trigger;

    public void ToClosed()
    {
        Closed = true;
        Trigger.enabled = true;
    }

    public void ToOpen()
    {
        Closed = false;
        Trigger.enabled = false;
    }
}
