using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class ConfirmInstrumentSeparation : MonoBehaviour
{
    [SerializeField]
    private GameObject finishButtonHolder;
    private InstrumentSpawner _instrumentSpawner;
    private InteractableInstrument[] _interactableInstruments;
    private InstrumentSeparation _instrumentSeparation;
    private Button _finishButton;
    private GameData _gameData;
    private int _currentLevel;

    private void Awake()
    {
        _instrumentSpawner = FindObjectOfType<InstrumentSpawner>();
        _gameData = DataManager.ReadJson("InstrumentSeparation");
        _currentLevel = (GameDataManager.Instance.currentLevel == 0) ? 1 : GameDataManager.Instance.currentLevel;
        _instrumentSeparation = InstrumentSeparation.Instance;
        _finishButton = finishButtonHolder.GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _interactableInstruments = _instrumentSpawner.SpawnInstruments().Select(obj => obj.GetComponent<InteractableInstrument>()).ToArray();
        
        foreach (var interactableInstrument in _interactableInstruments)
        {
            interactableInstrument.Button.onClick.AddListener(OnConfirmHearing);
        }
        
        _finishButton.onClick.AddListener(OnFinishButtonPressed);
        finishButtonHolder.SetActive(false);
    }

    private void OnDisable()
    {
        if(_interactableInstruments != null)
        {
            foreach (var interactableInstrument in _interactableInstruments)
            {
                interactableInstrument.Button.onClick.RemoveListener(OnConfirmHearing);
            }
        }
        
        _finishButton.onClick.RemoveListener(OnFinishButtonPressed);
        finishButtonHolder.SetActive(false);
    }

    private void OnConfirmHearing()
    {
        if (IsMissingSelections())
        {
            finishButtonHolder.SetActive(false);
            return;
        }
        
        finishButtonHolder.SetActive(true);
    }

    private void OnFinishButtonPressed()
    {
        if(_currentLevel == _gameData.level)
        {
            _gameData.level += 1;
        }
        _gameData.levelScore[_currentLevel - 1] = 3;
        DataManager.SaveDataToJson(_gameData, "InstrumentSeparation");
#region DataCollection
        JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Instrument Separation", DateTime.Now, 3, _currentLevel));

        List<SeparatedInstrumentData> separatedInstrumentData = new List<SeparatedInstrumentData>();
        for (int i = 0; i < _interactableInstruments.Length; i++)
        {
            separatedInstrumentData.Add(
                new SeparatedInstrumentData( 
                    _interactableInstruments[i].gameObject.name,
                    _interactableInstruments[i].CanBeHeard,
                    Vector3.Distance(Camera.main.transform.position, _interactableInstruments[i].transform.position),
                    new List<OtherInstrumentDistances>(
                        _interactableInstruments.Select(
                            otherInstrumentDistance => new OtherInstrumentDistances(
                                otherInstrumentDistance.gameObject.name,
                                Vector3.Distance(_interactableInstruments[i].transform.position, otherInstrumentDistance.transform.position)
                                )
                            )
                        )
                    ));
        }
        JsonManager.WriteDataToFile<InstrumentSeparationGameData>(
            new InstrumentSeparationGameData(
                _instrumentSpawner.chosenSong.songName, 
                _currentLevel,
                separatedInstrumentData
            ));

        _instrumentSeparation.EndGame();
#endregion
    }

    private bool IsMissingSelections() => _interactableInstruments.Any(i => !i.HasClickedOnce);

    private IEnumerator WaitForInstruments()
    {
        yield return new WaitForEndOfFrame();
        
        
    }
}
