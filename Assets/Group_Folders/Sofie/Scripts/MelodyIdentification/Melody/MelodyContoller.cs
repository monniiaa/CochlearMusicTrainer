using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MelodyContoller : MonoBehaviour
{
    public delegate void End();
    public event End EndEvent;
    
    public delegate void Match(bool match);
    public event Match MatchEvent;


    public int numCards = 8;

    public bool similarCards = false;
    public bool sameMelody = false;
    
    [SerializeField] MemoryCard originalCard;
    
    public MemoryCard firstRevealed;
    public MemoryCard secondRevealed;
    private int score = 0;
    private int currentScene;
    private List<int> matchId = new List<int>();
    
    private List<string> prefabs = new List<string>();
    private List<string> audioclips = new List<string>();
    private List<string> hapticClips = new List<string>();

    [SerializeField] private bool hapticsOn = false;
    CardSpawner cardSpawner;
    private MemoryCard[] cards;
    

    private void Awake()
    {
        cardSpawner = GetComponent<CardSpawner>();

    }

    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    public void SetDifficulty(int numCards, bool similarCards, bool sameMelody, bool hapticsOn)
    {
        this.numCards = numCards;
        this.similarCards = similarCards;
        this.sameMelody = sameMelody;
        this.hapticsOn = hapticsOn;
    }
    
        public void StartGame()
    {
        int[] cardsIdexes = GenerateCardVector(numCards);
        if(!hapticsOn) (audioclips, prefabs) = GetComponent<RandomizeInstruments>().SelectAndRandomizeCards(numCards, similarCards, sameMelody);
        else (audioclips, prefabs, hapticClips) = GetComponent<RandomizeInstruments>().SelectAndRandomizeCards(numCards, similarCards, sameMelody, hapticsOn );
        
        cards = cardSpawner.SpawnCards(numCards, originalCard);

        for (int i = 0; i < cards.Length; i++)
        {
            MemoryCard card = cards[i];

            int id = cardsIdexes[i];
            card._id = id;
            
            card.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(audioclips[id]);
            card.SetAndSpawnInstrument(Resources.Load<GameObject>(prefabs[id]));
            
            if (hapticsOn)
            {
                HapticClip hapticClip = Resources.Load<HapticClip>(hapticClips[id]);
                card.SetHaptics(hapticClip);
            }
        }
    }

    private int[] GenerateCardVector(int nCards)
    {
        int cardIdx = 0;

        int[] cardsVector = new int[nCards];
        
        for (int card = 0; card < cardsVector.Length; card++)
        {
            // Assign an increasing index for each couple of cards
            if (card % 2 == 0 && card > 1)
            {
                cardIdx++;

            }

            cardsVector[card] = cardIdx;
        }
        
        cardsVector = ShuffleArray(cardsVector);

        return cardsVector;
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = UnityEngine.Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
    public void CardRevealed(MemoryCard card)
    {
        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            if (firstRevealed != card)
            {
                secondRevealed = card;
                StartCoroutine(CheckMatch());
            }
            
        }
    }

    private IEnumerator CheckMatch()
    {
        if (firstRevealed.Id == secondRevealed.Id &&
            firstRevealed.transform.position != secondRevealed.transform.position &&
            !matchId.Contains(firstRevealed.Id))
        {
            matchId.Add(firstRevealed.Id);
            score++;
            print($"Score: {score}");

            firstRevealed.Reveal();
            secondRevealed.Reveal();

            firstRevealed.alreadyMatched = true;
            secondRevealed.alreadyMatched = true;

            firstRevealed.Matched();
            secondRevealed.Matched();
            new WaitForSeconds(1f);
            MatchEvent?.Invoke(true);

            if (score == numCards / 2)
            {
                yield return new WaitForSeconds(0.5f);
               // firstRevealed.audioSource.Stop();
               // secondRevealed.audioSource.Stop();
               // if (hapticsOn)
               // {
               //     firstRevealed.StopHaptics();
               //     secondRevealed.StopHaptics();
               // }
               CollectCardInfo();
               StartCoroutine(WaitToDestroyCards());
               EndEvent?.Invoke();
            }
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
                if(!matchId.Contains(firstRevealed.Id))
                {
                    firstRevealed.Unreveal();
                    secondRevealed.Unreveal();
                }
        }
        firstRevealed = null;
        secondRevealed = null;
    }

    private void CollectCardInfo()
    {
        foreach (MemoryCard card in cards)
        {
            JsonManager.WriteDataToFile<MelodyIdentificationGameData>(
                new MelodyIdentificationGameData(
                    card.instrument.gameObject.name,
                    card.numClicks,
                    FindObjectOfType<MemoryMelodyManager>().level,
                    card.audioSource.clip.name,
                    similarCards,
                    sameMelody,
                    hapticsOn
                ));
        }
        
    }

    IEnumerator WaitToDestroyCards()
    {
        yield return new WaitForSeconds(0.5f);
        cardSpawner.DestroyCards(cards);
    }
    
    
}
