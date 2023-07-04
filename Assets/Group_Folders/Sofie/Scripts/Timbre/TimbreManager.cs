using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using TMPro;
public class TimbreManager : LevelManager
{
    private int maxlevel = 4;

    private GameDataManager _gameDataManager;
    private DateTime startTime;
    public InstrumentSeparation ModeManager;

    [SerializeField] private GameObject[] instrumentFamilies;

    [SerializeField] private TextMeshProUGUI startingText;
    private int amountOfInstruments;

    private List<InstrumentBehavior> instrumentsPlaying = new List<InstrumentBehavior>();

    private bool numberOfInstrumentsMode = false;

    private int guesses;
    [SerializeField] private InstrumentsCanvas CorrectInstrumentsCanvas;

    void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;
        
        gameplayAudio = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData = DataManager.ReadJson(path);

        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        currentLevel = 1;
        Debug.Log(difficulty);
        SetMode();
        CorrectInstrumentsCanvas.gameObject.SetActive(false);
    }

    private void StartingInstructions()
    {
        if (!numberOfInstrumentsMode)
        {
            startingText.text = "Choose the " + amountOfInstruments + " instruments that are playing";
        }
        else
        {
            startingText.text = "How many instruments are playing?";
        }
    }

    private void Start()
    {
        StartRound();
    }

    public void InstrumentPicked(InstrumentBehavior instrument)
    {
        if (guesses > 0)
        {
            guesses--;
            if (instrumentsPlaying.Contains(instrument))
            {
                gameplayAudio.PlayOneShot(sucessAudio);
                instrument.CorrectAnimation(true);
                currentScore += 1;
            }
            else
            {
                gameplayAudio.PlayOneShot(failAudio);
        
            }
        }
        if (guesses == 0)
        {
            StartCoroutine(ShowCorrectAnswer());
        }
    }

    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                if (currentLevel == 1)
                {
                    PlayPickedInstruments(SetInstrumentsPlaying(2));
                    Debug.Log(instrumentsPlaying.Count);
                    guesses = instrumentsPlaying.Count;
                }
                else
                {
                    int randEasy = UnityEngine.Random.Range(1, 3);
                    PlayPickedInstruments(SetInstrumentsPlaying(randEasy));
                    guesses = instrumentsPlaying.Count;
                }
                break;
            case Difficulty.Medium:
                int randMedium = UnityEngine.Random.Range(2, 4);
                PlayPickedInstruments(SetInstrumentsPlaying(randMedium));
                guesses = instrumentsPlaying.Count;
                break;
            case Difficulty.Hard:
                int randHard = UnityEngine.Random.Range(2, 4);
                PlayPickedInstruments(SetInstrumentsPlaying(randHard, true));
                guesses = instrumentsPlaying.Count;
                break;
        }
        
    }

    public override void SetRoundFunctionality()
    {
        round++;
        if (round < 4)
        {
            StartRound();
        }
        else
        {
            StartCoroutine(End());
            if(currentLevel == gameData.level)
            {
                gameData.level = (currentLevel == maxLevel) ? gameData.level : currentLevel + 1;
            }          
            if(gameData.levelScore[currentLevel - 1] < currentScore)
            {
                gameData.levelScore[currentLevel - 1] = currentScore;
            }
            DataManager.SaveDataToJson(gameData, path);  
        }
    }

    public void CanvasEnd()
    {
        CorrectInstrumentsCanvas.gameObject.SetActive(false);
        EndRound();
    }
    protected override void EndRound()
    {
        StopPlayingInstrument();
        instrumentsPlaying.Clear();
    }
    

    IEnumerator End()
    {
        yield return new WaitForSeconds(0.7f);
    }


    public void InstrumentPicked(GameObject instrument)
    {
        EndRound();
        if (instrumentsPlaying.Contains(instrument.GetComponent<InstrumentBehavior>()))
        {
            gameplayAudio.PlayOneShot(sucessAudio);
            instrument.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore+= 1;
        }
        else
        {
            gameplayAudio.PlayOneShot(failAudio);
            instrument.GetComponent<MeshRenderer>().material = failMaterial;
        }
    }

    IEnumerator ShowCorrectAnswer()
    {
        yield return new WaitForSeconds(1f);
        CorrectInstrumentsCanvas.gameObject.SetActive(true);
        CorrectInstrumentsCanvas.SetCorrectInstrumentsText(instrumentsPlaying);
        
    }
    

    protected override void StartRound()
    {
        startTime = DateTime.Now;
        for(int i = 0; i < instrumentFamilies.Length; i++)
        {
            List<GameObject> instruments = instrumentFamilies[i].GetComponent<Family>().instruments;
            for (int j = 0; j < instruments.Count; j++)
            {
                instruments[j].GetComponent<InstrumentBehavior>().SetPickedState(false);
            }
        }
        SetDifficultyChanges();

    }
    
    

    /// <summary>
    /// Chooses random instruments to play.
    /// </summary>
    /// <param name="numberOfInstruments">Should be equal to or smaller than amount of instruments in scene ideally 1-5</param>
    /// <param name="sameFamily">Should instruments from the same family spawn?</param>
    private List<GameObject> SetInstrumentsPlaying(int numberOfInstruments, bool sameFamily =false)
    {
        List<GameObject> pickedFamily = new List<GameObject>();
        int rand = Random.Range(0, instrumentFamilies.Length);
        pickedFamily.Add(instrumentFamilies[rand]);

        List<GameObject> pickedInstruments = new List<GameObject>();
        
        int randI = Random.Range(0, pickedFamily[0].GetComponent<Family>().instruments.Count);
        pickedInstruments.Add(pickedFamily[0].GetComponent<Family>().instruments[randI]);
        pickedFamily[0].GetComponent<Family>().instruments.RemoveAt(randI);
        if (sameFamily)
        {
            for (int i = 1; i < numberOfInstruments ; i++)
            {
                if (pickedFamily[0].GetComponent<Family>().instruments.Count != 0)
                {
                    int randInstrument = Random.Range(0, pickedFamily[0].GetComponent<Family>().instruments.Count);
                    pickedInstruments.Add(pickedFamily[0].GetComponent<Family>().instruments[randInstrument]);
                    pickedFamily[0].GetComponent<Family>().instruments.RemoveAt(randInstrument);

                }
                else
                {
                    int randFamily = Random.Range(0, instrumentFamilies.Length);
                    GameObject pickedInstrument;
                    do
                    {
                         pickedInstrument= instrumentFamilies[randFamily];
                    } while (pickedFamily.Contains(instrumentFamilies[randFamily]));

                    pickedFamily.Add(pickedInstrument);
                    
                    int randInstrument = Random.Range(0, pickedFamily[0].GetComponent<Family>().instruments.Count);
                    pickedInstruments.Add(pickedFamily[pickedFamily.Count - 1 ].GetComponent<Family>().instruments[randInstrument]);
                    pickedFamily[pickedFamily.Count -1].GetComponent<Family>().instruments.RemoveAt(randInstrument);
                }
            }
        }
        else
        {
            
        }
        foreach (GameObject family in pickedFamily)
        {
            family.GetComponent<Family>().ResetInstrumentsInFamily();
        }
        
        return pickedInstruments;
    }


private void PlayPickedInstruments(List<GameObject> instrumentsToPlay)
    {
        foreach (GameObject instrument in instrumentsToPlay)
        {
            instrument.GetComponent<InstrumentBehavior>().SetClip();
            instrument.GetComponent<InstrumentBehavior>().Play();
            instrumentsPlaying.Add(instrument.GetComponent<InstrumentBehavior>());
        }
    }

private void StopPlayingInstrument()
{
    foreach (InstrumentBehavior instrument in instrumentsPlaying)
    {
        instrument.StopAudio();   
    }
}

    protected override void RestartLevel()
    {
        currentScore = 0;
        round = 1;
        StartRound();
    }
    
}
