using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastLocalizationHit : MonoBehaviour
{
    public float raycastDistance = 10f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayer))
            {
                if (hit.collider.gameObject.CompareTag("InteractablePrefabTag"))
                {
                    Animation animation = hit.collider.gameObject.GetComponent<Animation>();
                    animation.Play();
                }
            }
        }
    }
}




