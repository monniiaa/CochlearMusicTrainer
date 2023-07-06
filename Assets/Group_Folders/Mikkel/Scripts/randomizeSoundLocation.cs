using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    [SerializeField] private GameObject player;
    private GameObject prefabInstance;
    private Vector3 previousPosition;


    public GameObject SpawnSpeaker(GameObject prefab) =>
        Instantiate(prefab, GenerateRandomPoint(), Quaternion.identity);


    private Vector3 GenerateRandomPoint()
    {
        Bounds innerBounds = new Bounds(Camera.main.transform.position,
            new Vector3(minDistanceToPlayer, height, minDistanceToPlayer));
        Bounds outerBounds = new Bounds(Camera.main.transform.position, new Vector3(roomSize, height, roomSize));
        Vector3 randomPoint;
        do
        {
            randomPoint = new Vector3(
                Random.Range(outerBounds.min.x, outerBounds.max.x),
                Random.Range(outerBounds.min.y, outerBounds.max.y),
                Random.Range(outerBounds.min.z, outerBounds.max.z)
            );
        } while (randomPoint.x > innerBounds.min.x && randomPoint.x < innerBounds.max.x &&
                 randomPoint.y > innerBounds.min.y && randomPoint.y < innerBounds.max.y &&
                 randomPoint.z > innerBounds.min.z && randomPoint.z < innerBounds.max.z);

        return randomPoint;
    }

    public List<GameObject> SpawnSpeakers(int num, float minDistanceBetweenSpeakers, GameObject prefab)
    {
        List<Vector3> spawnPoints = GenerateRandomSpawnPoints(num, minDistanceBetweenSpeakers);
        Debug.Log("Spawnpoints: " + spawnPoints.Count);
        List<GameObject> speakers = new List<GameObject>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            speakers.Add(Instantiate(prefab, spawnPoints[i], Quaternion.identity));
        }

        return speakers;
    }

    private List<Vector3> GenerateRandomSpawnPoints(int numberOfPoints, float minDistance)
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        Bounds innerBounds = new Bounds(Camera.main.transform.position,
            new Vector3(minDistanceToPlayer, height, minDistanceToPlayer));
        Bounds outerBounds = new Bounds(Camera.main.transform.position, new Vector3(roomSize, height, roomSize));

        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 randomPoint;
            bool isValidPoint = false;

            // Generate a random point within the outer bounds
            do
            {
                do
                {
                    randomPoint = new Vector3(
                        Random.Range(outerBounds.min.x, outerBounds.max.x),
                        Random.Range(outerBounds.min.y, outerBounds.max.y),
                        Random.Range(outerBounds.min.z, outerBounds.max.z)
                    );
                } while (randomPoint.x > innerBounds.min.x && randomPoint.x < innerBounds.max.x &&
                         randomPoint.y > innerBounds.min.y && randomPoint.y < innerBounds.max.y &&
                         randomPoint.z > innerBounds.min.z && randomPoint.z < innerBounds.max.z);

                // Check if the point is within the inner bounds and not too close to existing spawn points
                isValidPoint = true;
                foreach (Vector3 point in spawnPoints)
                {
                    if (Vector3.Distance(randomPoint, point) < minDistance)
                    {
                        isValidPoint = false;
                        break;
                    }
                }
            } while (!isValidPoint);

            spawnPoints.Add(randomPoint);
        }

        return spawnPoints;
    }




#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireCube(transform.position, Vector3.one * minDistanceToPlayer);
        Gizmos.DrawWireCube(transform.position, new Vector3(roomSize, height, roomSize));
        

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
