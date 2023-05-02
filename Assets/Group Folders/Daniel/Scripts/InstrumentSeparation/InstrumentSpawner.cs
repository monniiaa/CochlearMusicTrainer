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
    public SongData chosenSong;
    private const string dataPath = "InstrumentSeparation";
    private GameDataManager _gameDataManager;

    private void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        _gameData = DataManager.ReadJson(dataPath);
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    public GameObject[] SpawnInstruments()
    {
        int songIndex = _gameDataManager.currentLevel - 1;
        songIndex = Mathf.Clamp(songIndex, 0, songs.Length - 1);
        SongData song = songs[songIndex];
        chosenSong = song;
        _grabbableInstruments = new GameObject[song.stems.Length];
        int startTimeSeconds = (int) ((song.startTime.x * 60) + song.startTime.y);
        int endTimeSeconds = (int) ((song.endTime.x * 60) + song.endTime.y);

        AudioSource[] audioSources = new AudioSource[song.stems.Length];

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _grabbableInstruments[i] = Instantiate(grabbablePrefab);
            _grabbableInstruments[i].gameObject.name = song.stems[i].instrumentData.instrument.ToString();
            GameObject confirmUI = Instantiate(confirmUIPrefab, _grabbableInstruments[i].transform);
            GameObject instrument = Instantiate(song.stems[i].instrumentData.instrumentPrefab, _grabbableInstruments[i].transform);
            GameObject soundSource = Instantiate(soundSourcePrefab, _grabbableInstruments[i].transform);
            GameObject attachmentPoint = new GameObject("AttachmentPoint");

            _grabbableInstruments[i].AddComponent<InteractableInstrument>();
            attachmentPoint.transform.SetParent(_grabbableInstruments[i].transform);

            BoxCollider instrumentHolderCollider = _grabbableInstruments[i].GetComponent<BoxCollider>();
            Bounds instrumentBounds = instrument.GetComponent<MeshRenderer>().bounds;
            if (instrumentBounds.size.magnitude <= 0.2f)
            {
                instrument.transform.localScale = new Vector3(2f, 2f, 2f);
                instrumentBounds = instrument.GetComponent<MeshRenderer>().bounds;
            }
            instrumentHolderCollider.size = instrumentBounds.size;
            instrumentHolderCollider.center = instrumentBounds.center;

            attachmentPoint.transform.position = instrumentHolderCollider.center;

            confirmUI.transform.position = new Vector3(0, instrumentBounds.max.y * _grabbableInstruments[i].transform.localScale.y + 0.1f, 0);
            _grabbableInstruments[i].transform.position = _spawnPoints[i].transform.position;

            audioSources[i] = soundSource.GetComponent<AudioSource>();
            audioSources[i].clip = song.stems[i].audioClip;
            audioSources[i].timeSamples = startTimeSeconds * audioSources[i].clip.frequency;
            audioSources[i].gameObject.SetActive(true);
        }

        StopAllCoroutines();
        StartCoroutine(CheckForSongEnd(audioSources, startTimeSeconds, endTimeSeconds));


        return _grabbableInstruments;
    }

    private void OnDisable()
    {
        if (_grabbableInstruments == null) return;

        foreach (var grabbableInstrument in _grabbableInstruments)
        {
            Destroy(grabbableInstrument);
        }
    }

    IEnumerator CheckForSongEnd(AudioSource[] audioSources, int startTimeSeconds, int endTimeSeconds)
    {
        while (true)
        {
            
            if (audioSources[0].timeSamples >= endTimeSeconds * audioSources[0].clip.frequency)
            {
                foreach (var audioSource in audioSources)
                {
                    audioSource.mute = true;
                    audioSource.timeSamples = startTimeSeconds * audioSource.clip.frequency;
                }

                yield return new WaitForSeconds(1.5f);

                foreach (var audioSource in audioSources)
                {
                    audioSource.mute = false;
                }
            }
            
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var spawner in spawners)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 0.1f);
        }
    }
#endif
}