using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrumstick : MonoBehaviour
{
    GameObject startTransform;
    Transform controller;
    static public bool pickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        startTransform = new GameObject();

        startTransform.transform.position = transform.position;
        startTransform.transform.rotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp == true)
        {
            transform.position = controller.position;
            transform.rotation = controller.rotation;
        }
        else
        {
            transform.position = startTransform.transform.position;
            transform.rotation = startTransform.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        pickedUp = true;
        controller = other.gameObject.transform;
    }
}
