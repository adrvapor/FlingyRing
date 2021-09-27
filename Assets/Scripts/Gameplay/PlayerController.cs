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
    public float inertiaCounterFactor = 0.5f;

    public float maxHorizontalSpeed = 3f;
    public float maxVerticalSpeed = 5f;

    public float minX = -10;
    public float maxX = 10;

    public Vector2 screenBounds;

    private Coroutine powerUpCoroutine;

    private Rigidbody Rigidbody;
    private ShieldPowerUp Shield;
    private PearlMultiplierPowerUp PearlMultiplier;

    private GameObject Ring;
    private Renderer RingRenderer;
    public RingList RingList;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Shield = GetComponentInChildren<ShieldPowerUp>();
        PearlMultiplier = GetComponentInChildren<PearlMultiplierPowerUp>();
        
        Ring = Instantiate(RingList.GetSelectedRing());
        Ring.transform.parent = this.transform;
        Ring.transform.localPosition = Vector3.zero;
        RingRenderer = Ring.GetComponentInChildren<MeshRenderer>();

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        screenBounds.x -= 0.3f;
        screenBounds.y -= 1.5f;

        GameManager.SetPlayer(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) FlingRight();
        if (Input.GetKeyDown(KeyCode.RightArrow)) FlingLeft();
    }

    void FixedUpdate()
    {
        GameManager.UpdateScore(Rigidbody.position.y);

        Rigidbody.velocity = new Vector3(Mathf.Clamp(Rigidbody.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed),
                                        Mathf.Clamp(Rigidbody.velocity.y, -maxVerticalSpeed, maxVerticalSpeed));

        Vector3 position = Rigidbody.position;

        if (position.y <= GameManager.GetScore() - screenBounds.y)
            GameManager.GameOver();

        if (- screenBounds.x >= position.x || position.x >= screenBounds.x)
            Rigidbody.velocity -= new Vector3(Rigidbody.velocity.x * bouncyness, 0, 0);

        position.x = Mathf.Clamp(position.x, -screenBounds.x, screenBounds.x);
        Rigidbody.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pearl")
        {
            other.gameObject.SetActive(false);
            GameManager.AddPearl(PearlMultiplier.IsPearl ? 2 : 1);
        }

        if (other.tag == "SuperPearl")
        {
            other.gameObject.SetActive(false);
            GameManager.AddPearl(PearlMultiplier.IsPearl ? 10 : 5);
        }

        if(!Shield.On)
            if (other.tag == "Urchin" || 
                other.tag == "Eel" ||
                other.tag == "Shark" ||
                other.tag == "Octopus" ||
                (other.tag == "Oyster" && other.gameObject.GetComponent<OysterDetector>().Closed))
            {
                gameObject.SetActive(false);
                GameManager.GameOver();
            }
    }

    public void FlingRight()
    {
        if(Settings.RumbleOn) Handheld.Vibrate();
        Rigidbody.velocity += Vector3.up * verticalImpulse + Vector3.right * horizontalImpulse 
            + (Rigidbody.velocity.x <= 0 ? new Vector3(-Rigidbody.velocity.x * inertiaCounterFactor, 0, 0) : Vector3.zero);
    }

    public void FlingLeft()
    {
        if (Settings.RumbleOn) Handheld.Vibrate();
        Rigidbody.velocity += Vector3.up * verticalImpulse + Vector3.right * -horizontalImpulse 
            + (Rigidbody.velocity.x >= 0 ? new Vector3(-Rigidbody.velocity.x * inertiaCounterFactor, 0, 0) : Vector3.zero);
    }

    public void ActivatePowerUp(PowerUpTypes type)
    {
        // We reset any power ups if they're active
        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);

            Shield.ResetShield();
            PearlMultiplier.ResetPearlRing();
            RingRenderer.enabled = true;
        }

        switch (type)
        {
            case PowerUpTypes.SHIELD:
                Shield.ActivateShield();
                powerUpCoroutine = StartCoroutine(Shield.DeactivateShield());
                break;

            case PowerUpTypes.DOUBLE_PEARLS:
                PearlMultiplier.ActivatePearlRing(RingRenderer);
                powerUpCoroutine = StartCoroutine(PearlMultiplier.DeactivatePearlRing(RingRenderer));
                break;
                
            default:
                break;
        }
    }
}