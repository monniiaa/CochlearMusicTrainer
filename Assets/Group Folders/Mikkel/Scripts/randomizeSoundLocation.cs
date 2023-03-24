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
    private int spawnedSpeaker;

    private GameObject player;
    private GameObject prefabInstance;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        
    }

    public GameObject SpawnSpeaker(GameObject prefab)
    {
            Vector3 randomPoint;
            do
            {
                float randomX = Random.Range(-roomSize / 2f, roomSize / 2f);
                float randomZ = Random.Range(-roomSize / 2f, roomSize / 2f);
                float randomY = Random.Range(-height / 2, height / 2);
                randomPoint = new Vector3(randomX, randomY, randomZ) + transform.position;
            } while (Vector3.Distance(randomPoint, player.transform.position) < minDistanceToPlayer && Vector3.Distance(randomPoint, player.transform.position) < minDistanceToPlayer);

            return Instantiate(prefab, randomPoint, Quaternion.identity);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one * minDistanceToPlayer);
        Gizmos.DrawWireCube(transform.position, new Vector3(roomSize, height, roomSize));
    }
}
