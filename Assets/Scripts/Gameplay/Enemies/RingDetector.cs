using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDetector : MonoBehaviour
{
    public Animator animator;
    public GameObject detectText;

    public float defaultSpeed;
    public float detectSpeed = 2;

    private void Start()
    {
        detectText.SetActive(false);
        animator.speed = defaultSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.speed = detectSpeed;
            detectText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            animator.speed = defaultSpeed;
            detectText.SetActive(false);
        }
    }
}
