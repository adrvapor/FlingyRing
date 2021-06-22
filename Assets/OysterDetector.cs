using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OysterDetector : MonoBehaviour
{
    public bool Closed = false;

    public void ToClosed()
    {
        Closed = true;
    }

    public void ToOpen()
    {
        Closed = false;
    }
}
