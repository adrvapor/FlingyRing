using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingList : MonoBehaviour
{
    [System.Serializable]
    public class RingType
    {
        public string key;
        public GameObject ring;
        public bool unlocked;
    }

    public List<RingType> Rings;

    public GameObject GetRingByKey(string key)
        => Rings.First(ring => ring.key == key).ring ?? Rings[0].ring;

    public GameObject GetSelectedRing()
        => GetRingByKey(UserDataManager.GetSelectedRing());
}
