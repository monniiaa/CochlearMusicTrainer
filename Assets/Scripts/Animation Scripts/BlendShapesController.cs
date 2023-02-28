using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapesController : MonoBehaviour
{
    private SkinnedMeshRenderer body;

    private bool closing;
    private bool opening;

    private float maximum = 100.0f;
    private float minimum = 0.0f;
    static float weight = 0.0f;

    public float speed = 20.0f;

    private void Start()
    {
        body = GetComponent<SkinnedMeshRenderer>();

        StartCoroutine(Blinking(4.0f));
    }

    private IEnumerator Blinking(float time)
    {
        yield return new WaitForSeconds(time);
        opening = false;
        closing = true;
    }

    private void Update()
    {
        if(closing)
        {
            // Close eyes
            body.SetBlendShapeWeight(0, Mathf.Lerp(minimum, maximum, weight));

            weight += speed * Time.deltaTime;

            // When eyes are completely closed swap minimum and maximum (a blend shape weight of 100 now means open eyes) and set opening to true
            if (weight > 1.0f && !opening)
            {
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
                weight = 0.0f;
                opening = true;
            }

            // When eyes are completely open swap minimum and maximum (default) and set closing to false, reset the blend shape weight, and start a new coroutine
            if (weight > 1.0f && opening)
            {
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
                weight = 0.0f;
                closing = false;
                body.SetBlendShapeWeight(0, 0);
                StartCoroutine(Blinking(4.0f));
            }
        } 
    }
}
