using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private LocalizationManager localizationManager;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        localizationManager = FindObjectOfType<LocalizationManager>();
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
        localizationManager.PickTarget(this.gameObject);
        
    }

    public void TriggerDestroyState()
    {
        animator.SetTrigger("Destroy");
    }
}
