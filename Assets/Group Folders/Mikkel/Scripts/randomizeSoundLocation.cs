using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizeSoundLocation : MonoBehaviour
{
    //public GameObject prefab;
    public float minDistanceToPlayer = 5f;
    public float roomSize = 20f;
    public float height = 10f;
    public const int MaxSpeakers = 3;
    public bool visualizeRandomness;
    private int spawnedSpeaker;
    [SerializeField]
    private GameObject player;
    private GameObject prefabInstance;
    private Vector3 previousPosition;


    public GameObject SpawnSpeaker(GameObject prefab) => Instantiate(prefab, GenerateRandomPoint(), Quaternion.identity);
    
    
    private Vector3 GenerateRandomPoint()
    {      
        Bounds innerBounds = new Bounds(Camera.main.transform.position, new Vector3(minDistanceToPlayer, height, minDistanceToPlayer));
        Bounds outerBounds = new Bounds(Camera.main.transform.position, new Vector3(roomSize, height, roomSize));
        Vector3 randomPoint;  
        do
        {
            randomPoint = new Vector3(
                Random.Range(outerBounds.min.x, outerBounds.max.x),
                Random.Range(outerBounds.min.y, outerBounds.max.y),
                Random.Range(outerBounds.min.z, outerBounds.max.z)
                );
        }
        while (randomPoint.x > innerBounds.min.x && randomPoint.x < innerBounds.max.x &&
                randomPoint.y > innerBounds.min.y && randomPoint.y < innerBounds.max.y &&
                randomPoint.z > innerBounds.min.z && randomPoint.z < innerBounds.max.z);
        
        return randomPoint;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        /*
        Gizmos.DrawWireCube(transform.position, Vector3.one * minDistanceToPlayer);
        Gizmos.DrawWireCube(transform.position, new Vector3(roomSize, height, roomSize));
        */

        Bounds innerBounds = new Bounds(Camera.main.transform.position, new Vector3(minDistanceToPlayer, height, minDistanceToPlayer));
        Bounds outerBounds = new Bounds(Camera.main.transform.position, new Vector3(roomSize, height, roomSize));
        if(visualizeRandomness)
        {
            for (int i = 0; i < 30; i++)
            {
                Gizmos.DrawSphere(GenerateRandomPoint(), 0.1f);
            }
        }
        
        Gizmos.DrawWireCube(innerBounds.center, innerBounds.size);
        Gizmos.DrawWireCube(outerBounds.center, outerBounds.size);
    }
#endif
}
