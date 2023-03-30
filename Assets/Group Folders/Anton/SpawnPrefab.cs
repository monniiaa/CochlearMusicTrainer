using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public GameObject _prefab;

    public void SpawnThePrefab()
    {
        Instantiate(_prefab, transform.position, Random.rotation);
    }
}
