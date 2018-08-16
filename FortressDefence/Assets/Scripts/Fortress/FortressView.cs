using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressView : MonoBehaviour {


    public Transform[] archersPosition;

    private Animator animator;

    public void GetHit()
    {
        
    }

    public void SetHealth(float percent)
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        animator.SetFloat("healthPercent", percent);
    }
}
