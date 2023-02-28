using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerGirl : MonoBehaviour
{
    AnimationClip sittingStretching;

    bool offset = false;

    void Start()
    {
        sittingStretching = GetComponent<Animation>().GetClip("SittingStretching (Girl)");
        StartCoroutine(Animate());        
    }

    IEnumerator Animate()
    {
        if (!offset)
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 2.5f));
            GetComponent<Animation>().Stop("SittingIdle (Girl)");
            GetComponent<Animation>().Play("SittingIdle (Girl)");
            offset = true;
            StartCoroutine(Animate());
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 300.0f));
            GetComponent<Animation>().Stop("SittingIdle (Girl)");
            GetComponent<Animation>().Play("SittingStretching (Girl)");
            yield return new WaitForSeconds(sittingStretching.length);
            GetComponent<Animation>().Stop("SittingStretching (Girl)");
            GetComponent<Animation>().Play("SittingIdle (Girl)");
            StartCoroutine(Animate());
        }
    }
}
