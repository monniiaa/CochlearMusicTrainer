using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVerticalPos : MonoBehaviour
{
    private float prevHit;
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit);
        if(prevHit!=hit.distance){
            transform.position = new Vector3(transform.position.x, transform.position.y-hit.distance+0.03f, transform.position.z);
        }
    
        prevHit = hit.distance;

}
}
