using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstrumentSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] instruments;
    
    [SerializeField] private GameObject[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        instruments = Resources.LoadAll<GameObject>("Instruments");
    }

    private void Start()
    {
        SpawnInstruments();
    }

    public void SpawnInstruments()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            int randomIndex = Random.Range(0, instruments.Length);
            GameObject spawnedInstrument = Instantiate(instruments[randomIndex]);
            spawnedInstrument.transform.position = spawnPoint.transform.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var spawner in spawners)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 0.1f);
        }
    }
}
