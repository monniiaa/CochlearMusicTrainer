using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    private SelectionOutline[] outlines;
    public GameObject selected;
    
    // Start is called before the first frame update
    void Awake()
    {
        outlines = FindObjectsOfType<SelectionOutline>();
        Debug.Log(outlines.Length);
    }

    private void OnEnable()
    {
        foreach (SelectionOutline outline in outlines)
        {
            outline.OnOutlineChange += Math;
        }
    }
    private void OnDisable()
    {
        foreach (SelectionOutline outline in outlines)
        {
            outline.OnOutlineChange -= Math;
        }
    }

    private void Math(GameObject selectedObj)
    {
        foreach (SelectionOutline seloutline in outlines)
        {
            if (selectedObj == seloutline.gameObject)
            {
                selected = selectedObj;
                continue;
            }
            seloutline.outline.enabled = false;
        }
    }
}
