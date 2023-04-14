using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionOutline : MonoBehaviour
{
    public Outline outline;
    public GameObject selected;
    private OutlineManager manager;
    public delegate void outlineChanged(GameObject gameObject);
    public event outlineChanged OnOutlineChange;

    // Start is called before the first frame update
    void Awake()
    {
        manager = FindObjectOfType<OutlineManager>();
        outline = GetComponent<Outline>();
    }

   public void SetOutline()
    {
        outline.enabled = !outline.enabled;
        OnOutlineChange.Invoke(gameObject);
    }
}
