using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChange : MonoBehaviour
{
    
    // Start is called before the first frame update


    public void MakeTransparent(Renderer modelRenderer, float alpha)
    {
        Color color = modelRenderer.material.color;
        color.a = alpha;
        modelRenderer.material.color = color;
        
    }
}
