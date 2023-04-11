using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    private Camera _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var lookPos = _target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }
}
