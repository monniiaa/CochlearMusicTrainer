using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLookAt : MonoBehaviour
{
    Transform target;

    private void Start()
    {
        target = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(target.position);
        Vector3 rot = transform.eulerAngles;
        rot.x = 0;
        rot.y = transform.eulerAngles.y + 90;
        rot.z = 90;
        transform.eulerAngles = rot;

    }
}
