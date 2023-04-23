using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscriminationManager : LevelManager
{
    private Oscillator[] melodies;
    public static int levelInterval { get; private set; }
    public static int levelSequenceLength { get; private set; }
    public static float levelNoteTime { get; private set; }
    private int intervalDistance = 4;
    private Oscillator originalMelody;

    private Oscillator pickedMelody;
    public InstrumentSeparation ModeManager;
    public GameObject[] starAnimation = new GameObject[4];
    
    private void Start()
    {
        ModeManager = InstrumentSeparation.Instance;
        melodies = GameObject.FindObjectsOfType<Oscillator>();
        path = "PitchDiscrimination";
        StartRound();
        gameData = DataManager.ReadJson(path);
        currentLevel = gameData.level;
        SetDifficultyChanges();
        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
        {
            
        }
    }

    public void RestartLevel()
    {
        currentLevel--;
        round = 1;
        currentScore = 0;
        foreach (Oscillator osc in melodies)
        {
            osc.gameObject.SetActive(true);
        }
        StartRound();
    }
    
    public void SetOriginalMelody()
    {
        foreach(Oscillator melody in melodies)
        {
            if (melody.gameObject.CompareTag("Original"))
            {
                originalMelody = melody;
                melody.CreateStartNote();
            }
        }
    }

    public void SetEquivalentMelody()
    {
        int rand;
        do
        {
            rand = Random.Range(0, melodies.Length);
        } while (melodies[rand].CompareTag("Original"));

        melodies[rand].CreateStartNote(originalMelody.startNote);
        melodies[rand].gameObject.tag = "Equivalent";
    }

    public void SetDissimilarMelodies()
    {
        foreach(Oscillator melody in melodies)
        {
            if (!melody.gameObject.CompareTag("Original") && !melody.CompareTag("Equivalent"))
            {
                do
                {
                    melody.CreateStartNote();
                } while (melody.startNote < originalMelody.startNote + intervalDistance && melody.startNote > originalMelody.startNote - intervalDistance
                && melody.startNote <= (originalMelody.startNote + intervalDistance) % originalMelody.notes.Length);
            }
        }
    }

    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                levelInterval = Random.Range(3, 5);
                levelSequenceLength = Random.Range(2, 4);
                break;
            case Difficulty.Medium:
                levelInterval = Random.Range(1, 2);
                levelSequenceLength = Random.Range(3, 5);
                break;
            case Difficulty.Hard:
                levelInterval = Random.Range(1, 2);
                levelSequenceLength = Random.Range(3, 6);
                break;
        }
        intervalDistance = Random.Range(0, 2);
        levelNoteTime = 0.4f;
    }

    public void PickMelody(Oscillator pickedMelody)
    {
        this.pickedMelody = pickedMelody;
    }
    protected override void EndRound()
    {
        foreach (Oscillator osc in melodies)
        {
            osc.DestroyAnimation();
            
        }
        if (pickedMelody.startNote == originalMelody.startNote)
        {
            gameplayAudio.PlayOneShot(sucessAudio);
            pickedMelody.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore++;
            gameData.levelScore[currentLevel - 1] = currentScore;
            pickedMelody.tag = "Untagged";
            pickedMelody = null;
        }
        else 
        {
            gameplayAudio.PlayOneShot(failAudio);
            pickedMelody.GetComponent<MeshRenderer>().material = failMaterial;
            pickedMelody = null;
        }
    }

    protected override void StartRound()
    {
        StartCoroutine(test());
        SetDifficultyChanges();
        SetOriginalMelody();
        SetEquivalentMelody();
        SetDissimilarMelodies();

    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.7f);
        foreach (Oscillator osc in melodies)
        {
            osc.GetComponent<MeshRenderer>().material = osc.mat;
        }
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(0.7f);
        foreach (Oscillator osc in melodies)
        {
            osc.gameObject.SetActive(false);
        }
        ModeManager.EndGame();
        switch ((currentScore)) 
        {   
            case 1 :
                starAnimation[1].SetActive(true);
                break;
            case 2:
                starAnimation[2].SetActive(true);
                break;
            case 3:
                starAnimation[3].SetActive(true);
                break;
            default:   
                starAnimation[0].SetActive(true);
                break;
        }

    }
    public override void SetRoundFunctionality()
    {
        round++;
        EndRound();
       if(round < 4)
        {
            StartRound();
        }
        else
        {
           StartCoroutine(End());
           currentLevel++;
           gameData.level = currentLevel;
           DataManager.SaveDataToJson(gameData, path);
        }
    }
}
