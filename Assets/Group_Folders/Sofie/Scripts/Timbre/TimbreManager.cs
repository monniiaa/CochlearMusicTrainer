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

    private bool numberOfInstrumentsMode = false;

    void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;

        gameplayAudio = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData = DataManager.ReadJson(path);

        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        SetMode();

      //  foreach (GameObject star in starAnimation)
       // {
       //     star.gameObject.SetActive(false);
       // }

        
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

    protected override void SetDifficultyChanges()
    {

    }

    public override void SetRoundFunctionality()
    {

    }

    protected override void EndRound()
    {

    }

    protected override void StartRound()
    {
        List<GameObject> instrumentsPlaying = SetInstrumentsPlaying(3, true);
        PlayPickedInstruments(instrumentsPlaying);
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
        foreach (GameObject family in instrumentFamilies)
        {
            family.GetComponent<Family>().ResetInstrumentsInFamily();
        }

        return pickedInstruments;
    }


private void PlayPickedInstruments(List<GameObject> instrumentsPlaying)
    {
        foreach (GameObject instrument in instrumentsPlaying)
        {
            instrument.GetComponent<InstrumentBehavior>().SetClip();
            instrument.GetComponent<InstrumentBehavior>().Play();
        }
    }


    protected override void RestartLevel()
    {
        currentScore = 0;
        round = 1;
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
