using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetPickedState(false);
    }


    public void SetPickedState(bool state)
    {
        animator.SetBool("Picked", state);
    }
}
