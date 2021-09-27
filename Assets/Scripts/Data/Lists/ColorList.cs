using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorList : MonoBehaviour
{
    [System.Serializable]
    public class ColorKey
    {
        public string key;
        public Color color;
    }

    public List<ColorKey> colors;
}
