using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherView : MonoBehaviour {

    private Animator animator;

    private int shootTriggerHash;

	void Start () {
        animator = GetComponent<Animator>();
        shootTriggerHash = Animator.StringToHash("shoot");
	}
	
    public void Shoot(){
        animator.SetTrigger(shootTriggerHash);
    }
}
