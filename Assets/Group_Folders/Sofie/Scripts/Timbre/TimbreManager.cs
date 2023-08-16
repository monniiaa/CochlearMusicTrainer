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

    private InstrumentSeparation ModeManager;

    private bool numberOfInstrumentsMode = false;
    private int guesses;
    [SerializeField] private InstrumentsCanvas CorrectInstrumentsCanvas;
    [SerializeField] private TextMeshProUGUI instructionText;
    private bool wasCorrect;
    
    [SerializeField] private Canvas amountOfInstrumentsCanvas;
    [SerializeField] private GameObject[] easyModes = new GameObject[2];
    [SerializeField] private GameObject[] mediumModes = new GameObject[2];
    bool haptics = false;

    private InstrumentPicker instrumentPicker;

    private List<InstrumentBehavior> instumentsGuessed = new List<InstrumentBehavior>();

    void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;
        instrumentPicker = GetComponent<InstrumentPicker>();
        
        gameplayAudio = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData = DataManager.ReadJson(path);

        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        SetMode();
        if(currentLevel <2)
        {
            easyModes[0].SetActive(true);
            easyModes[1].SetActive(false);
        }
        else if(currentLevel >= 2 && currentLevel <= 3)
        {
            easyModes[1].SetActive(true);
            easyModes[0].SetActive(false);
        }

        if (currentLevel == 4)
        {
            mediumModes[0].SetActive(true);
            mediumModes[1].SetActive(false);
        }
        else if(currentLevel == 5 || currentLevel == 6)
        {
            mediumModes[0].SetActive(false);
            mediumModes[1].SetActive(true);
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
            if (haptics == true)
            {
                if(instrumentPicker.instrumentsPlayingSound.Count > 1)
                {
                    instructionText.text = "Flere instrumenter spiller, vælg det der spiller og vibrere i controlleren";
                } else
                {
                    instructionText.text = "Vælg det instrument der matcher vibrationerne i controlleren";
                }
            }
            else
            {
                if (instrumentPicker.instrumentsPlayingSound.Count > 1)
                {
                    instructionText.text = "Vælg de " + instrumentPicker.instrumentsPlayingSound.Count + " instrumenter der spiller";
                }
                else
                {
                    instructionText.text = "Vælg det instrument der spiller";
                }
            }
        }
        else
        {
            instructionText.text = "";
        }
    }

    private void Start()
    {
        StartRound();
    }
    
    public void InstrumentPicked(GameObject instrument)
    {
        instumentsGuessed.Add(instrument.GetComponent<InstrumentBehavior>());
        if (guesses > 0)
        {
            guesses--;
            if (!haptics)
            {
                InstrumentBehavior guess = instrument.GetComponent<InstrumentBehavior>();
                if (instrumentPicker.instrumentsPlayingSound.Contains(guess))
                {
                    CorrectAnswer(guess);
                }
                else
                {
                    WrongAnswer(guess);
                }
            }
            else
            {
                HapticsBehavior guess = instrument.GetComponent<HapticsBehavior>();
                if (instrumentPicker.instrumentPlayingHaptics == guess)
                {
                    CorrectAnswer(instrument.GetComponent<InstrumentBehavior>());
                    wasCorrect = true;
                }
                else
                {
                    WrongAnswer(instrument.GetComponent<InstrumentBehavior>());
                    wasCorrect = false;
                }
            }

        }
        if (guesses == 0)
        {
            if (!haptics)
            {
                var areEquivalent = (instumentsGuessed.Count == instrumentPicker.instrumentsPlayingSound.Count) && !instumentsGuessed.Except(instrumentPicker.instrumentsPlayingSound).Any();
                    if(areEquivalent) wasCorrect = true;
                    else wasCorrect = false;
                    Debug.Log(areEquivalent);
            }
            StartCoroutine(ShowCorrectAnswer());
        }
    }

    private void CorrectAnswer(InstrumentBehavior soundAndAnimation)
    {
        gameplayAudio.PlayOneShot(sucessAudio);
        soundAndAnimation.CorrectAnimation(true);
        soundAndAnimation.DestroyAnimation(true);
    }

    private void WrongAnswer(InstrumentBehavior soundAndAnimation)
    {
        gameplayAudio.PlayOneShot(failAudio);
        soundAndAnimation.DestroyAnimation(true);
    }
    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                int amount;
                if (currentLevel < 3) amount = 1;
                else amount = UnityEngine.Random.Range(1, 3);
                SoundOnlyMode(amount);
                break;
            case Difficulty.Medium:
                int randMedium;
                if (currentLevel == 4)
                {
                    randMedium = UnityEngine.Random.Range(1, 4);
                    AmountOfInstrumentsMode(randMedium);
                } else if (currentLevel == 5  || currentLevel == 6)
                {
                   // numberOfInstrumentsMode = false;
                    HapticsAndSoundMode(2);
                }
                break;
            case Difficulty.Hard:
                if (currentLevel == 7)
                {
                    HapticsOnlyMode(0);
                }
                else if (currentLevel == 8)
                {
                    int randRoundClip = UnityEngine.Random.Range(0, 6);
                    HapticsOnlyMode(randRoundClip);;
                }
                else
                {
                    SoundOnlyMode(3);
                }
                break;
        }
    }

    private void SoundOnlyMode(int amount)
    {
        instrumentPicker.PickInstrumentsPlayingSound(amount);
        instrumentPicker.PlayPickedInstrumentsSound();
        guesses = amount;
    }

    private void HapticsAndSoundMode(int amountPlayingSound)
    {
        haptics = true;
        instrumentPicker.PickInstrumentsPlayingSoundAndHaptics(amountPlayingSound);
        instrumentPicker.PlayPickedInstrumentsSound();
        instrumentPicker.PlayPickedInstrumentHaptics();
        guesses = 1;
    }

    private void AmountOfInstrumentsMode(int amount)
    {
        numberOfInstrumentsMode = true;
        instrumentPicker.PickInstrumentsPlayingSound(amount);
        instrumentPicker.PlayPickedInstrumentsSound();
        amountOfInstrumentsCanvas.gameObject.SetActive(true);
        amountOfInstrumentsCanvas.GetComponent<AmountOfInstrumentsCanvas>().SetOptions(3);
    }

    private void HapticsOnlyMode(int roundClip)
    {
        haptics = true;
        guesses = 1;
        instrumentPicker.SetRoundClip(roundClip);
        instrumentPicker.PickHapticsInstrument(true);
        instrumentPicker.PlayPickedInstrumentHaptics();
    }
    
    
    public override void SetRoundFunctionality()
    {
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
        string[] choosenInstruments = new string[instumentsGuessed.Count];
        string[] correctInstruments;

        if (!haptics)
        {
            correctInstruments = new string[instrumentPicker.instrumentsPlayingSound.Count];
            for(int i = 0; i < correctInstruments.Length; i++)
            {
                correctInstruments[i] = instrumentPicker.instrumentsPlayingSound[i].name;
            }
        }
        else
        {
            correctInstruments = new string[1];
            correctInstruments[0] = instrumentPicker.instrumentPlayingHaptics.GetComponent<InstrumentBehavior>().name;
        }

        for(int i = 0; i < instumentsGuessed.Count; i++)
        {
            choosenInstruments[i] = instumentsGuessed[i].name;
        }

        JsonManager.WriteDataToFile<InstrumentIdentificationGameData>(
            new InstrumentIdentificationGameData(
                DateTime.Now, 
                DateTime.Now - startTime,
                choosenInstruments,
                correctInstruments,
                wasCorrect,
                currentLevel,
                round
            )
        );
        instrumentPicker.StopInstrumentsPlaying(haptics);
        instumentsGuessed.Clear();
        wasCorrect = false;
    }
    

    public void End()
    {
        EndRound();
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
    

    IEnumerator ShowCorrectAnswer()
    {
        instrumentPicker.StopInstrumentsPlaying(haptics);
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
        if(!haptics) CorrectInstrumentsCanvas.SetCorrectInstrumentsText(instrumentPicker.instrumentsPlayingSound, wasCorrect , numberOfInstrumentsMode);
        else CorrectInstrumentsCanvas.SetCorrectInstrumentsText(instrumentPicker.instrumentPlayingHaptics.GetComponent<InstrumentBehavior>(), wasCorrect);

    }
    

    protected override void StartRound()
    {
        if (!numberOfInstrumentsMode)
        {
            instrumentPicker.ResetInstruments();
        }
        startTime = DateTime.Now;
        SetDifficultyChanges();
        StartingInstructions();

    }
    
    protected override void RestartLevel()
    {
        if(numberOfInstrumentsMode) amountOfInstrumentsCanvas.gameObject.SetActive(true);
        instrumentPicker.ResetSoundAndHapticsCombination();
        currentScore = 0;
        round = 1;
    }

    public void AmountOfInstrumentsPicked(int amount)
    {
         amountOfInstrumentsCanvas.gameObject.SetActive(false);
        if (amount == instrumentPicker.instrumentsPlayingSound.Count)
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
