using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float shieldDuration = 10f;
    public bool On;
    
    private SpriteRenderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        On = false;
        renderer.enabled = On;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateShield()
    {
        On = true;
        renderer.enabled = On;

        StartCoroutine(DeactivateShield());
    }

    IEnumerator DeactivateShield()
    {
        yield return new WaitForSeconds(shieldDuration);

        for (int i = 0; i < 10; i++)
        {
            enabled = !enabled;
            renderer.enabled = enabled;
            yield return new WaitForSeconds(.4f);
        }

        On = false;
        renderer.enabled = On;
    }
}
