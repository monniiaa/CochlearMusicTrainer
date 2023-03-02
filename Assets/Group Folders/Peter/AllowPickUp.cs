using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DVRInput.Get(DVRInput.Button.Two))
        {
            if (PickUpDrumstick.pickedUp == true)
            {
                PickUpDrumstick.pickedUp = false;
            }
        }
    }
}
