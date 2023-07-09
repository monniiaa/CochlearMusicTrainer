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
    private int maxlevel = 10;

    private GameDataManager _gameDataManager;
    private DateTime startTime;

    [SerializeField] private GameObject[] instrumentFamilies;

    private InstrumentSeparation ModeManager;

    private List<InstrumentBehavior> instrumentsPlaying = new List<InstrumentBehavior>();

    private bool numberOfInstrumentsMode = false;
    private bool sameFamily;
    private int guesses;
    [SerializeField] private InstrumentsCanvas CorrectInstrumentsCanvas;
    [SerializeField] private TextMeshProUGUI instructionText;
    private bool wasCorrect;
    private int wrongAnswer;
    private List<GameObject> previousCombination = new List<GameObject>();

    void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;
        
        gameplayAudio = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData = DataManager.ReadJson(path);

        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;

        SetMode();
        CorrectInstrumentsCanvas.gameObject.SetActive(false);
        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
    }

    private void StartingInstructions()
    {
        instructionText.gameObject.SetActive(true);
        if (!numberOfInstrumentsMode)
        {
            if (instrumentsPlaying.Count > 1)
            {
                instructionText.text = "Vælg de " + instrumentsPlaying.Count + " instrumenter der spiller";
            }
            else
            {
                instructionText.text = "Vælg det instrument der spiller";
            }
            
        }
        else
        {
            instructionText.text = "How many instruments are playing?";
        }
    }

    private void Start()
    {
        instrumentFamilies = new GameObject[4];
        instrumentFamilies[0] = GameObject.Find("Brass");
        instrumentFamilies[1] = GameObject.Find("Woodwind");
        instrumentFamilies[2] = GameObject.Find("Percussion");
        instrumentFamilies[3] = GameObject.Find("Strings");
        StartCoroutine(WaitForStart());
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(1f);
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
                instrument.DestroyAnimation(true);
                wasCorrect = true;
            }
            else
            {
                gameplayAudio.PlayOneShot(failAudio);
                wasCorrect = false;
                instrument.DestroyAnimation(true);
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
                sameFamily = false;
                if (currentLevel == 1)
                {
                    PlayPickedInstruments(SetInstrumentsPlaying(1));
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
                int randMedium = UnityEngine.Random.Range(2, 3);
                PlayPickedInstruments(SetInstrumentsPlaying(randMedium));
                guesses = instrumentsPlaying.Count;
                break;
            case Difficulty.Hard:
                sameFamily = true;
                int randHard = UnityEngine.Random.Range(2, 4);
                PlayPickedInstruments(SetInstrumentsPlaying(randHard, sameFamily));
                guesses = instrumentsPlaying.Count;
                break;
        }
        
    }
    private void WriteDataToJson(DateTime time, TimeSpan timeTakenToChooseInstrument, string chosenInstrument, bool correctInstrument, bool sameFamily, int level, int round)
    {
        JsonManager.WriteDataToFile<InstrumentIdentificationGameData>(
            new InstrumentIdentificationGameData(
                time,
                timeTakenToChooseInstrument,
                chosenInstrument,
                correctInstrument,
                sameFamily,
                level,
                round
            )
        );
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
        previousCombination.Clear();
        previousCombination = instrumentsPlaying.Select(x => x.gameObject).ToList();
        instrumentsPlaying.Clear();
    }
    

    public void End()
    {
        ModeManager.EndGame();
        ShowStar(currentScore);
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
        instructionText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        CorrectInstrumentsCanvas.gameObject.SetActive(true);
        if (round < 3)
        {
            CorrectInstrumentsCanvas.SetNextButtonBehavior(false);
        }
        else
        {
            CorrectInstrumentsCanvas.SetNextButtonBehavior(true);
        }

        if(wasCorrect) currentScore++;
        CorrectInstrumentsCanvas.SetCorrectInstrumentsText(instrumentsPlaying, wasCorrect);
        wasCorrect = false;

    }
    

    protected override void StartRound()
    {
        for (int i = 0; i < instrumentFamilies.Length; i++)
        {
            foreach (GameObject instrument in instrumentFamilies[i].GetComponent<Family>().instruments)
            {
                instrument.GetComponent<InstrumentBehavior>().SetPickedState(false);
                instrument.GetComponent<InstrumentBehavior>().DestroyAnimation(false);
            }
        }
        startTime = DateTime.Now;
        SetDifficultyChanges();
        StartingInstructions();

    }
    
    

    /// <summary>
    /// Chooses random instruments to play.
    /// </summary>
    /// <param name="numberOfInstruments">Should be equal to or smaller than amount of instruments in scene ideally 1-5</param>
    /// <param name="sameFamily">Should instruments from the same family spawn?</param>
    private List<InstrumentBehavior> SetInstrumentsPlaying(int numberOfInstruments, bool sameFamily =false)
    {
        GameObject fam;
        InstrumentBehavior pickedInstrument1;
        int rand;
        int randI;
        do
        {
            rand = Random.Range(0, instrumentFamilies.Length);
            fam = instrumentFamilies[rand];
            randI = Random.Range(0, fam.GetComponent<Family>().instruments.Count);
            pickedInstrument1 = fam.GetComponent<Family>().instruments[randI].GetComponent<InstrumentBehavior>();
        }while (previousCombination.Contains(pickedInstrument1.gameObject));
        
        
        List<InstrumentBehavior> pickedInstruments = new List<InstrumentBehavior>();
        List<GameObject> pickedFamily = new List<GameObject>();
        pickedInstruments.Add(pickedInstrument1);
        pickedFamily.Add(fam);
        
        if (sameFamily)
        {
            for (int i = 1; pickedInstruments.Count < numberOfInstruments ; i++)
            {
                if (pickedFamily[0].GetComponent<Family>().instruments.Count != 0)
                {
                    int randInstrument = Random.Range(0, pickedFamily[0].GetComponent<Family>().instruments.Count);
                    if(!pickedInstruments.Contains(pickedFamily[0].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>())) pickedInstruments.Add(pickedFamily[0].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>());
                }
                else
                {
                    int randFamily;
                    GameObject pickedInstrument;
                    do
                    {
                        randFamily = Random.Range(0, instrumentFamilies.Length);
                         pickedInstrument= instrumentFamilies[randFamily];
                    } while (pickedFamily.Contains(instrumentFamilies[randFamily]));

                    pickedFamily.Add(pickedInstrument);
                    
                    int randInstrument = Random.Range(0, pickedFamily[pickedFamily.Count - 1].GetComponent<Family>().instruments.Count);
                    if(!pickedInstruments.Contains(pickedFamily[pickedFamily.Count - 1 ].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>()))pickedInstruments.Add(pickedFamily[pickedFamily.Count - 1 ].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>());
                }
            }
        }
        else
        {
            for (int i = 1; pickedInstruments.Count < numberOfInstruments; i++)
            {
                int randFamily;
                GameObject fam1;
                do
                {
                    randFamily = Random.Range(0, instrumentFamilies.Length);
                    fam1= instrumentFamilies[randFamily];
                } while (pickedFamily.Contains(instrumentFamilies[randFamily]) && previousCombination.Contains(instrumentFamilies[randFamily]));

                pickedFamily.Add(fam1);
                    
                int randInstrument = Random.Range(0, pickedFamily[pickedFamily.Count -1 ].GetComponent<Family>().instruments.Count);
                if(!pickedInstruments.Contains(pickedFamily[pickedFamily.Count - 1].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>()))pickedInstruments.Add(pickedFamily[pickedFamily.Count - 1].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>());
            }
        }

        return pickedInstruments;
    }


private void PlayPickedInstruments(List<InstrumentBehavior> instrumentsToPlay)
    {
        int roundClip = Random.Range(0,6);
        for(int i = 0; i < instrumentsToPlay.Count;i++)
        {
            instrumentsToPlay[i].SetClip(roundClip);
            instrumentsToPlay[i].Play();
            instrumentsPlaying.Add(instrumentsToPlay[i]);
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
        instrumentsPlaying.Clear();
        previousCombination.Clear();
        currentScore = 0;
        round = 1;
    }

    public void Restart()
    {
        RestartLevel();
        CorrectInstrumentsCanvas.gameObject.SetActive(false);
        StartRound();
    }
}
