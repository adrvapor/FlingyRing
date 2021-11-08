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
        public Sprite render;
    }

    public List<RingType> Rings;

    public GameObject GetRingByKey(string key)
        => Rings.First(ring => ring.key == key).ring ?? Rings[0].ring;

    public Sprite GetRenderByKey(string key)
        => Rings.First(ring => ring.key == key).render ?? Rings[0].render;

    public GameObject GetSelectedRing()
        => GetRingByKey(UserDataManager.GetSelectedRing());
}
