using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleController : MonoBehaviour
{
    public PowerUpTypes Type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<PlayerController>() is PlayerController playerController)
        {
            playerController.ActivatePowerUp(Type);
            StartCoroutine("Disappear");
        }
    }

    IEnumerator Disappear()
    {
        bool enabled = true;

        var renderer = this.GetComponent<Renderer>();
        var childrenRenderers = this.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < 10; i++)
        {
            enabled = !enabled;
            renderer.enabled = enabled;
            foreach(var childRenderer in childrenRenderers) childRenderer.enabled = enabled;
            yield return new WaitForSeconds(.2f);
        }

        this.gameObject.transform.gameObject.SetActive(false);
    }
}
