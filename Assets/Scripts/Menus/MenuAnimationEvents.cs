using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimationEvents : MonoBehaviour
{
    public RingGalleryController RingGalleryController;
    private Animator Animator;

    public void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void RingGalleryAnimation()
    {
        if(Animator.GetCurrentAnimatorStateInfo(0).speed > 0)
            RingGalleryController.EnterGallery();
    }
}
