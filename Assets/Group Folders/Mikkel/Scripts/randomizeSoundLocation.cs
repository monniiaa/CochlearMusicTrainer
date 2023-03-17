using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizeSoundLocation : MonoBehaviour
{
    public GameObject prefab;
    public float minDistanceToPlayer = 5f;
    public float roomSize = 20f;
    public float height = 10f;
    public const int MaxSpeakers = 1;
    private int spawnedSpeaker;

    private GameObject player;
    private GameObject prefabInstance;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (spawnedSpeaker >= MaxSpeakers) return;
        if (prefabInstance == null)
        {
            float randomX = Random.Range(-roomSize / 2f, roomSize / 2f);
            float randomZ = Random.Range(-roomSize / 2f, roomSize / 2f);
            float randomY = Random.Range(-height/2, height/2);
            Vector3 randomPoint = new Vector3(randomX, randomY, randomZ) + transform.position;
            if (Vector3.Distance(randomPoint, player.transform.position) >= minDistanceToPlayer)
            {
                prefabInstance = Instantiate(prefab, randomPoint, Quaternion.identity);
                spawnedSpeaker++;
            }
        }
        else if (Vector3.Distance(prefabInstance.transform.position, player.transform.position) < minDistanceToPlayer)
        {
            Destroy(prefabInstance);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one * minDistanceToPlayer);
        Gizmos.DrawWireCube(transform.position, new Vector3(roomSize, height, roomSize));
    }
}
