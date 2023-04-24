using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    private SelectionOutline[] outlines;
    public GameObject selected;
    
    // Start is called before the first frame update
    void Awake()
    {
        outlines = FindObjectsOfType<SelectionOutline>(true);
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

    public void ClearAllSelections()
    {
        Debug.Log("Cleared all selections");
        selected = null;
        foreach (SelectionOutline seloutline in outlines)
        {
            if (seloutline.outline == null) continue;
            seloutline.outline.enabled = false;
        }
    }

    private void Math(GameObject selectedObj)
    {
        foreach (SelectionOutline seloutline in outlines)
        {
            if (seloutline.outline == null) continue;
            if (selectedObj == seloutline.gameObject)
            {
                selected = selectedObj;
                Debug.Log(selected.name + " is now selected");
                continue;
            }
            seloutline.outline.enabled = false;
        }
    }
}
