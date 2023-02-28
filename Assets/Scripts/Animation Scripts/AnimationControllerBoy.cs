using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerBoy : MonoBehaviour
{
    AnimationClip sittingStretching;
    AnimationClip sittingDabbing;

    bool offset = false;

    // This script plays animations offset using a coroutine. 
    private void Start()
    {
        sittingStretching = GetComponent<Animation>().GetClip("SittingStretching (Boy)");
        sittingDabbing = GetComponent<Animation>().GetClip("SittingDabbing (Boy)");
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        if(!offset)
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 2.5f));
            GetComponent<Animation>().Stop("SittingIdle (Boy)");
            GetComponent<Animation>().Play("SittingIdle (Boy)");
            offset = true;
            StartCoroutine(Animate());
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 300.0f));
            int i = (int)Random.Range(0.0f, 11.0f);

            if(i < 8)
            {
                GetComponent<Animation>().Stop("SittingIdle (Boy)");
                GetComponent<Animation>().Play("SittingStretching (Boy)");
                yield return new WaitForSeconds(sittingStretching.length);
                GetComponent<Animation>().Stop("SittingStretching (Boy)");
                GetComponent<Animation>().Play("SittingIdle (Boy)");
                StartCoroutine(Animate());
            }

            if(i > 8)
            {
                GetComponent<Animation>().Stop("SittingIdle (Boy)");
                GetComponent<Animation>().Play("SittingDabbing (Boy)");
                yield return new WaitForSeconds(sittingDabbing.length);
                GetComponent<Animation>().Stop("SittingDabbing (Boy)");
                GetComponent<Animation>().Play("SittingIdle (Boy)");
                StartCoroutine(Animate());
            }
            
        }
    }
}
