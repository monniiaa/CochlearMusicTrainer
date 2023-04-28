using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Instrument
{
    Trumpet,
    Trombone,
    PiccoloFlute,
    Clarinet,
    Saxophone,
    Violin,
    Cello,
    Piano,
    SnareDrum,
    Vocals,
    Bass,
    Other
}

public enum InstrumentFamily
{
    Woodwind = 0,
    Brass = 1,
    String = 2,
    PitchedPercussion = 3,
    Other = 4
}

public class InstrumentSpawner : MonoBehaviour
{
    [SerializeField] private SongData[] songs;
    [SerializeField] private GameObject grabbablePrefab;
    [SerializeField] private GameObject soundSourcePrefab;
    [SerializeField] private GameObject confirmUIPrefab;
    private GameData _gameData;
    private GameObject[] _spawnPoints;
    private AudioClip[] _audioClip;
    private GameObject[] _grabbableInstruments;
    private bool hasPlayed;
    private const string dataPath = "InstrumentSeparation";

    private void Awake()
    {
        songs = Resources.LoadAll<SongData>("Songs");
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    private void OnEnable()
    {
        _gameData = DataManager.ReadJson(dataPath);
        SpawnInstruments();
    }

    public void SpawnInstruments()
    {
        if(hasPlayed) _gameData.level--;
        DataManager.SaveDataToJson(_gameData, dataPath);

        int songIndex = _gameData.level - 1;
        if(songIndex > songs.Length - 1) songIndex = songs.Length - 1;
        SongData song = songs[songIndex];
        _grabbableInstruments = new GameObject[song.stems.Length];

        
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _grabbableInstruments[i] = Instantiate(grabbablePrefab);
            GameObject confirmUI = Instantiate(confirmUIPrefab, _grabbableInstruments[i].transform);
            GameObject instrument = Instantiate(song.stems[i].instrumentData.instrumentPrefab, _grabbableInstruments[i].transform);
            GameObject soundSource = Instantiate(soundSourcePrefab, _grabbableInstruments[i].transform);
            GameObject attachmentPoint = new GameObject("AttachmentPoint");

            _grabbableInstruments[i].AddComponent<InteractableInstrument>();
            attachmentPoint.transform.SetParent(_grabbableInstruments[i].transform);

            BoxCollider instrumentHolderCollider = _grabbableInstruments[i].GetComponent<BoxCollider>();
            Bounds instrumentBounds = instrument.GetComponent<MeshRenderer>().bounds;
            Debug.Log($"{song.stems[i].instrumentData.instrumentPrefab.name}: {instrumentBounds.size.magnitude}");
            if(instrumentBounds.size.magnitude <= 0.2f) 
            {
                instrument.transform.localScale = new Vector3(2f, 2f, 2f);
                instrumentBounds = instrument.GetComponent<MeshRenderer>().bounds;
            }
            instrumentHolderCollider.size = instrumentBounds.size;
            instrumentHolderCollider.center = instrumentBounds.center;
            
            attachmentPoint.transform.position = instrumentHolderCollider.center;

            confirmUI.transform.position = new Vector3(0, instrumentBounds.max.y * _grabbableInstruments[i].transform.localScale.y + 0.1f, 0);
            _grabbableInstruments[i].transform.position = _spawnPoints[i].transform.position;
            
            AudioSource audioSource = soundSource.GetComponent<AudioSource>();
            audioSource.clip = song.stems[i].audioClip;

            audioSource.timeSamples += audioSource.clip.samples/2; // Start halfway through the clip
            audioSource.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if(_grabbableInstruments == null) return;
        foreach (var grabbableInstrument in _grabbableInstruments)
        {
            Destroy(grabbableInstrument);
        }
        hasPlayed = true;
    }

    private GameObject GetRandomInstrument(IReadOnlyList<GameObject> instruments) => instruments[Random.Range(0, instruments.Count)];

    private GameObject[] GetRandomInstrumentFamily()
    {
        int randomInstrumentFamily = Random.Range(0, Enum.GetValues(typeof(InstrumentFamily)).Length);
        return Resources.LoadAll<GameObject>($"Instruments/{((InstrumentFamily) randomInstrumentFamily).ToString()}");
    }

    private void OnDrawGizmosSelected()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var spawner in spawners)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 0.1f);
        }
    }
}
