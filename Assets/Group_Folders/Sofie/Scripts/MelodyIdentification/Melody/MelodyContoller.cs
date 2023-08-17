using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyContoller : MonoBehaviour
{
    public int numCards = 8;

    private float offSetx = 200f;
    private float offSety = 200f;

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
    
    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    private void Start()
    {
        int[] cardsIdexes = GenerateCardVector(numCards);
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
            secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private string CheckMatch()
    {
        throw new NotImplementedException();
    }
}
