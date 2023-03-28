using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionOutline : MonoBehaviour
{
    Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
    }

   public void SetOutline()
    {
        outline.enabled = !outline.enabled;
    }

    public void ClearOutline()
    {
        outline.enabled = false;
    }
}
