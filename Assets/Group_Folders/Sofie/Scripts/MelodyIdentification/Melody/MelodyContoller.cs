using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using UnityEngine;

public class MelodyContoller : MonoBehaviour
{
    public int numCards = 8;

    private float offSetx = 2f;
    private float offSety = 2f;

    private int gridRows = 2;
    private int gridCols = 4;
    
    public bool similarCards = false;
    public bool sameMelody = false;
    
    [SerializeField] MemoryCard originalCard;
    
    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;
    private int score = 0;
    private int currentScene;
    private List<int> matchId = new List<int>();
    
    private List<string> sprites = new List<string>();
    private List<string> audioclips = new List<string>();
    private List<string> hapticClips = new List<string>();

    [SerializeField] private Canvas gameCanvas;
    
    [SerializeField] private bool hapticsOn = false;
    
    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    private void Start()
    {
        int[] cardsIdexes = GenerateCardVector(numCards);
        if(!hapticsOn) (audioclips, sprites) = GetComponent<RandomizeInstruments>().SelectAndRandomizeCards(numCards, similarCards, sameMelody);
        else (audioclips, sprites, hapticClips) = GetComponent<RandomizeInstruments>().SelectAndRandomizeCards(numCards, similarCards, sameMelody, hapticsOn );
        gridCols = numCards / gridRows;

        originalCard.transform.position = new Vector3((gridCols - 1), 1, 0);
        
        Vector3 startPos = originalCard.transform.position;

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }
                int index = j * gridCols + i;
                int id = cardsIdexes[index];

                card._id = id;

                card.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprites[id]);
                card.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(audioclips[id]);
                if (hapticsOn)
                {
                    HapticClip hapticClip = Resources.Load<HapticClip>(hapticClips[id]);
                    card.SetHaptics(hapticClip);
                }

                
                float posX = (offSetx * i) + startPos.x;
                float posY = -(offSety * j) + startPos.y;
                card.transform.parent = gameCanvas.transform;
                card.transform.localScale = new Vector3(22, 22, 110);
               // Vector3 offset = new Vector3(-10, -10, 0);
                card.transform.position = new Vector3(gameCanvas.transform.position.x + posX,gameCanvas.transform.position.y + posY, gameCanvas.transform.position.y + startPos.z) ;
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
            if (card != firstRevealed)
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
            //TODO: Play success feedback audio

            if (score == (gridCols * gridRows) / 2)
            {
                yield return new WaitForSeconds(0.5f);
                //TODO: Show end screen
                firstRevealed.audioSource.Stop();
                secondRevealed.audioSource.Stop();
                if (hapticsOn)
                {
                    firstRevealed.StopHaptics();
                    secondRevealed.StopHaptics();
                }
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
        //TODO: JSON file with recorded game information run when pressing either restart or homepage
    }
}
