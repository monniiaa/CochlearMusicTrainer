using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private SoundLocalization soundLocalization;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        soundLocalization = FindObjectOfType<SoundLocalization>();
    }

    public void SetPickedState(bool picked)
    {
        animator.SetBool("Picked", picked);
    }
    
    public void SetCorrectState(bool correct)
    {
        animator.SetBool("Correct", correct);
    }

    public void Picked()
    {
        soundLocalization.PickTarget(this.gameObject);
        
    }

    public void TriggerDestroyState()
    {
        animator.SetTrigger("Destroy");
    }
}
