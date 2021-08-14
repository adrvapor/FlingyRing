using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float shieldDuration = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnShieldOn()
    {
        gameObject.SetActive(true);

    }

    IEnumerable TurnShieldOff()
    {
        yield return new WaitForSeconds(shieldDuration);
        gameObject.SetActive(false);
    }
}
