using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{    
    public float horizontalImpulse = 1f;
    public float verticalImpulse = 2f;
    public float bouncyness = 1.6f;

    public float maxHorizontalSpeed = 3f;
    public float maxVerticalSpeed = 5f;

    public float minX = -10;
    public float maxX = 10;

    public Vector2 screenBounds;

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        screenBounds.x -= 0.3f;
        screenBounds.y -= 1.5f;

        GameManager.SetPlayer(this);
    }

    void FixedUpdate()
    {
        GameManager.UpdateScore(rigidbody.position.y);

        rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed),
                                        Mathf.Clamp(rigidbody.velocity.y, -maxVerticalSpeed, maxVerticalSpeed));

        Vector3 position = rigidbody.position;

        if (position.y <= GameManager.GetScore() - screenBounds.y)
            GameManager.GameOver();

        if (- screenBounds.x >= position.x || position.x >= screenBounds.x)
            rigidbody.velocity -= new Vector3(rigidbody.velocity.x * bouncyness, 0, 0);

        position.x = Mathf.Clamp(position.x, -screenBounds.x, screenBounds.x);
        rigidbody.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pearl")
        {
            other.gameObject.SetActive(false);
            GameManager.AddPearl();
        }

        if (other.tag == "SuperPearl")
        {
            other.gameObject.SetActive(false);
            GameManager.AddPearl(5);
        }

        if (other.tag == "Urchin" || other.tag == "Eel" || (other.tag == "Oyster" && other.gameObject.GetComponent<OysterDetector>().Closed))
        {
            //other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            GameManager.GameOver();
        }
    }

    public void FlingRight()
    {
        if(Settings.RumbleOn) Handheld.Vibrate();
        rigidbody.velocity += Vector3.up * verticalImpulse + Vector3.right * horizontalImpulse;
    }

    public void FlingLeft()
    {
        if (Settings.RumbleOn) Handheld.Vibrate();
        rigidbody.velocity += Vector3.up * verticalImpulse + Vector3.right * -horizontalImpulse;
    }

    //public void PushLeftButton() => holdingLeft = true;
    //public void PushRightButton() => holdingRight = true;
    //public void ReleaseLeftButton() => holdingLeft = false;
    //public void ReleaseRightButton() => holdingRight = false;
}
