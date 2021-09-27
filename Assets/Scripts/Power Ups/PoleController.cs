using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleController : MonoBehaviour
{
    public PowerUpTypes Type;
    private int PowerUpTypesCount = Enum.GetValues(typeof(PowerUpTypes)).Length;

    public int Orientation;

    public float Speed = 0.5f;
    public float MaxSpeed = 1.2f;

    private void FixedUpdate()
    {
        transform.position += (Vector3.right * Mathf.Clamp(Speed + transform.position.y/200, 0, MaxSpeed)  * Orientation * Time.deltaTime);
        if (Orientation * transform.position.x > 4) Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<PlayerController>() is PlayerController playerController)
        {
            playerController.ActivatePowerUp(Type);
            StartCoroutine(Disappear());
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

        Orientation = 0;
        Destroy(this.gameObject);
    }

    public void SpawnPole(float y)
    {
        int plusOrMinus = UnityEngine.Random.Range(0, 2) * 2 - 1;

        Type = (PowerUpTypes)UnityEngine.Random.Range(0, PowerUpTypesCount);
        Orientation = plusOrMinus;

        transform.position += new Vector3(
            -plusOrMinus * 3.2f,
            y - transform.position.y,
            0);

        gameObject.SetActive(true);
    }
}
