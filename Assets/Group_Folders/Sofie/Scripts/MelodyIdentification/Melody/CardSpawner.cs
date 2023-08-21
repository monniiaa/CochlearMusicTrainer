using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPointsHolder = new GameObject[2];

    private void SetActiveSpawnPoints(int amount)
    {
        if (amount == 8)
        {
            spawnPointsHolder[0].SetActive(true);
            spawnPointsHolder[1].SetActive(false);
        } else if (amount == 10)
        {
            spawnPointsHolder[0].SetActive(false);
            spawnPointsHolder[1].SetActive(true);
        }
        else
        {
            Debug.Log("Wrong amount of cards");
        }
    }
    public MemoryCard[] SpawnCards(int amount, MemoryCard originalCard)
    {
        SetActiveSpawnPoints(amount);
        
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");  
        MemoryCard[] cards = new MemoryCard[amount];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            MemoryCard card =Instantiate(originalCard, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation, spawnPoints[i].transform);
            cards[i] = card;
            cards[i].spawnpoint = spawnPoints[i];
        }
        
        return cards;
    }
    
    public void DestroyCards(MemoryCard[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Destroy(cards[i].instrument);
            Destroy(cards[i].gameObject);
        }
    }
    
}
