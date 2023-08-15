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
    [SerializeField] private Canvas amountOfInstrumentsCanvas;
    [SerializeField] private GameObject[] easyModes = new GameObject[2];
    private int roundClip;
    bool haptics = false;

    void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;
        
        gameplayAudio = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData = DataManager.ReadJson(path);

        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        currentLevel = 7;

        SetMode();
        if(currentLevel <=2)
        {
            easyModes[0].SetActive(true);
            easyModes[1].SetActive(false);
        }
        else if(currentLevel == 3)
        {
            easyModes[1].SetActive(true);
            easyModes[0].SetActive(false);
        }
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
            instructionText.text = "";
        }
    }

    private void Start()
    {
        instrumentFamilies = new GameObject[4];
        instrumentFamilies[0] = GameObject.Find("Brass");
        instrumentFamilies[1] = GameObject.Find("Woodwind");
        instrumentFamilies[2] = GameObject.Find("Percussion");
        instrumentFamilies[3] = GameObject.Find("Strings");
        StartRound();
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(1.4f);
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
                int amount;
                List<InstrumentBehavior> instruments;
                if (currentLevel == 1)
                {
                    amount = 1;

                }
                else
                {
                    amount = UnityEngine.Random.Range(1, 3);
                }
                instruments = SetInstrumentsPlaying(amount);
                PlayPickedInstrumentsSound(instruments);
                guesses = instrumentsPlaying.Count;
                break;
            case Difficulty.Medium:
                numberOfInstrumentsMode = true;
                int randMedium;
                randMedium = UnityEngine.Random.Range(1, 4);
                PlayPickedInstrumentsSound(SetInstrumentsPlaying(randMedium));
                break;
            case Difficulty.Hard:
                if (currentLevel == 7)
                {
                    haptics = true;
                    amount = 2;
                    instruments = SetInstrumentsPlaying(amount);
                    PlayPickedInstrumentsSound(instruments);
                    int rand = Random.Range(0, amount);
                    InstrumentBehavior temp = instrumentsPlaying[rand];
                    instrumentsPlaying.Clear();
                    instrumentsPlaying.Add(temp);
                    PlayPickedInstrumentHaptics(instrumentsPlaying[0].gameObject.GetComponent<HapticsBehavior>());
                    guesses = 1;
                }
                if (currentLevel == 10)
                {
                    //HapticsOnly = true;
                    amount = 1;
                    instruments = SetInstrumentsPlaying(amount);
                    instrumentsPlaying = instruments;
                    PlayPickedInstrumentHaptics(instruments[0].gameObject.GetComponent<HapticsBehavior>());
                    guesses = amount;
                }
                
              //  sameFamily = true;
              //  int randHard = UnityEngine.Random.Range(2, 4);
              //  PlayPickedInstrumentsSound(SetInstrumentsPlaying(randHard, sameFamily));
                guesses = instrumentsPlaying.Count;
                break;
        }
        
    }
    private void WriteDataToJson(DateTime time, TimeSpan timeTakenToChooseInstrument, string chosenInstrument, bool correctInstrument, int level, int round)
    {
        JsonManager.WriteDataToFile<InstrumentIdentificationGameData>(
            new InstrumentIdentificationGameData(
                time,
                timeTakenToChooseInstrument,
                chosenInstrument,
                correctInstrument,

                level,
                round
            )
        );
    }
    public override void SetRoundFunctionality()
    {
        if (numberOfInstrumentsMode)
        {
            amountOfInstrumentsCanvas.gameObject.SetActive(true);
        }
        round++;
        if (round < 4)
        {
            StartRound();
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
        if(currentLevel == gameData.level)
        {
            gameData.level = (currentLevel == maxLevel) ? gameData.level : currentLevel + 1;
        }          
        if(gameData.levelScore[currentLevel - 1] < currentScore)
        {
            gameData.levelScore[currentLevel - 1] = currentScore;
        }
        DataManager.SaveDataToJson(gameData, path);  
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
        InstrumentBehavior[] temp = GameObject.FindObjectsOfType<InstrumentBehavior>();
        foreach (InstrumentBehavior instrument in temp)
        {
            instrument.StopAudio();
            instrument.GetComponent<HapticsBehavior>().StopHaptics();
        }
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
        CorrectInstrumentsCanvas.SetCorrectInstrumentsText(instrumentsPlaying, wasCorrect , numberOfInstrumentsMode);
        wasCorrect = false;

    }
    

    protected override void StartRound()
    {
        if (!numberOfInstrumentsMode)
        {
            for (int i = 0; i < instrumentFamilies.Length; i++)
            {
                foreach (GameObject instrument in instrumentFamilies[i].GetComponent<Family>().instruments)
                {
                    instrument.GetComponent<InstrumentBehavior>().SetPickedState(false);
                    instrument.GetComponent<InstrumentBehavior>().DestroyAnimation(false);
                }
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

            for (int i = 1; pickedInstruments.Count < numberOfInstruments; i++)
            {
                int randFamily;
                GameObject fam1;

                randFamily = Random.Range(0, instrumentFamilies.Length);
                fam1= instrumentFamilies[randFamily];


                pickedFamily.Add(fam1);
                    
                int randInstrument = Random.Range(0, pickedFamily[pickedFamily.Count -1 ].GetComponent<Family>().instruments.Count);
                if(!pickedInstruments.Contains(pickedFamily[pickedFamily.Count - 1].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>()))pickedInstruments.Add(pickedFamily[pickedFamily.Count - 1].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>());
            }

            return pickedInstruments;
    }
    
private void PlayPickedInstrumentsSound(List<InstrumentBehavior> instrumentsToPlay)
{
    roundClip = 0; //Random.Range(0,6);
        for(int i = 0; i < instrumentsToPlay.Count;i++)
        {
            AudioClip clip = instrumentsToPlay[i].GetClip(roundClip);
            instrumentsToPlay[i].PlayClip(clip);
            instrumentsPlaying.Add(instrumentsToPlay[i]);
        }
    }

private void PlayPickedInstrumentHaptics(HapticsBehavior hapticsToPlay)
{
    hapticsToPlay.PlayHapticsClip(roundClip);
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
        if(numberOfInstrumentsMode) amountOfInstrumentsCanvas.gameObject.SetActive(true);
        instrumentsPlaying.Clear();
        previousCombination.Clear();
        currentScore = 0;
        round = 1;
    }

    public void AmountOfInstrumentsPicked(int amount)
    {
         amountOfInstrumentsCanvas.gameObject.SetActive(false);
        if (amount == instrumentsPlaying.Count)
        {
            gameplayAudio.PlayOneShot(sucessAudio);
            wasCorrect = true;
        }
        else
        {
            gameplayAudio.PlayOneShot(failAudio);
            wasCorrect = false;
        }
        
        StartCoroutine(ShowCorrectAnswer());
    }

    public void Restart()
    {
        RestartLevel();
        CorrectInstrumentsCanvas.gameObject.SetActive(false);
        StartRound();
    }
}
