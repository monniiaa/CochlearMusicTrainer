using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMelody : MonoBehaviour
{

    private Vector3 position;

    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        
       mat = GetComponent<MeshRenderer>().material;
    }

    public void Reset()
    {
        transform.position = position;
        GetComponent<MeshRenderer>().material = mat;
        GetComponent<Animator>().SetBool("Destroy", false);
    }

    public IEnumerator StartReset()
    {
        yield return new WaitForSeconds(0.5f);
        Reset();
    }
    
}
