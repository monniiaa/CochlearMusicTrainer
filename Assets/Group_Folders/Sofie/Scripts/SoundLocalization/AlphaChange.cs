using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChange : MonoBehaviour
{
    [SerializeField] private Material[] material = new Material[4];
    [SerializeField] private Color[] originalColor = new Color[4];
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < material.Length;i++)
        {
            originalColor[i] = material[i].color;
            Color temp = material[i].color;
            temp.a = 0.2f;
            material[i].color = temp;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < material.Length; i++)
        {
            material[i].color = originalColor[i];
        }
    }
}
