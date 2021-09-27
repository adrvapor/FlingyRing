using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlMultiplierPowerUp : MonoBehaviour
{
    public float pearlDuration = 8f;
    public bool IsPearl;

    private SpriteRenderer CircleRenderer;
    private MeshRenderer[] PearlRenderers;
    private TrailRenderer TrailRenderer;

    public void Start()
    {
        CircleRenderer = GetComponentInChildren<SpriteRenderer>();
        PearlRenderers = GetComponentsInChildren<MeshRenderer>();
        TrailRenderer = GetComponentInChildren<TrailRenderer>();
        ResetPearlRing();
    }

    public void ActivatePearlRing(Renderer DefaultRing)
    {
        IsPearl = true;
        DefaultRing.enabled = false;
        RenderersEnabled(true);
    }

    public IEnumerator DeactivatePearlRing(Renderer DefaultRing)
    {
        yield return new WaitForSeconds(pearlDuration);

        for (int i = 0; i < 10; i++)
        {
            DefaultRing.enabled = i % 2 == 1;
            RenderersEnabled(i % 2 == 0);
            yield return new WaitForSeconds(.4f);
        }

        ResetPearlRing();
    }

    public void ResetPearlRing()
    {
        IsPearl = false;
        RenderersEnabled(false);
    }

    public void RenderersEnabled(bool enabled)
    {
        foreach (var r in PearlRenderers)
            r.enabled = enabled;
        CircleRenderer.enabled = enabled;
        TrailRenderer.enabled = enabled;
    }
}
