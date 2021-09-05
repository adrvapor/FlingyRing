using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    public float shieldDuration = 8f;
    public bool On;
    
    private SpriteRenderer ShieldRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ShieldRenderer = GetComponent<SpriteRenderer>();
        ResetShield();
    }

    public void ActivateShield()
    {
        On = true;
        ShieldRenderer.enabled = On;
    }

    public IEnumerator DeactivateShield()
    {
        yield return new WaitForSeconds(shieldDuration);

        for (int i = 0; i < 10; i++)
        {
            //enabled = !enabled;
            ShieldRenderer.enabled = i % 2 == 0;
            yield return new WaitForSeconds(.4f);
        }

        ResetShield();
    }

    public void ResetShield()
    {
        On = false;
        ShieldRenderer.enabled = On;
    }
}
