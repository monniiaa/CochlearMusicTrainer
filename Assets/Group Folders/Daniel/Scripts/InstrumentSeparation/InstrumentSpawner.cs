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
    private GameObject[] _spawnPoints;
    private AudioClip[] _audioClip;
    private GameObject[] _grabbableInstruments;

    private void Awake()
    {
        songs = Resources.LoadAll<SongData>("Songs");
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    private void OnEnable()
    {
        SpawnInstruments();
    }

    public void SpawnInstruments()
    {
        SongData randomSong = songs[Random.Range(0, songs.Length)];
        _grabbableInstruments = new GameObject[randomSong.stems.Length];
        
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _grabbableInstruments[i] = Instantiate(grabbablePrefab);
            GameObject confirmUI = Instantiate(confirmUIPrefab, _grabbableInstruments[i].transform);
            GameObject instrument = Instantiate(randomSong.stems[i].instrumentData.instrumentPrefab, _grabbableInstruments[i].transform);
            GameObject soundSource = Instantiate(soundSourcePrefab, _grabbableInstruments[i].transform);
            GameObject attachmentPoint = new GameObject("AttachmentPoint");
            _grabbableInstruments[i].AddComponent<InteractableInstrument>();
            attachmentPoint.transform.SetParent(_grabbableInstruments[i].transform);
            BoxCollider instrumentHolderCollider = _grabbableInstruments[i].GetComponent<BoxCollider>();
            Bounds instrumentBounds = instrument.GetComponent<MeshRenderer>().bounds;
            instrumentHolderCollider.size = instrumentBounds.size;
            instrumentHolderCollider.center = instrumentBounds.center;
            attachmentPoint.transform.position = instrumentHolderCollider.center;
            confirmUI.transform.position = new Vector3(0, instrumentBounds.max.y + 0.1f, 0);
            _grabbableInstruments[i].transform.position = _spawnPoints[i].transform.position;
            
            AudioSource audioSource = soundSource.GetComponent<AudioSource>();
            audioSource.clip = randomSong.stems[i].audioClip;
            audioSource.timeSamples += audioSource.clip.samples/2; // Start halfway through the clip
            audioSource.gameObject.SetActive(true);
        }
        
        
        /*
        foreach (var spawnPoint in _spawnPoints)
        {
            GameObject[] instruments = GetRandomInstrumentFamily();
            GameObject spawnedInstrument = Instantiate(GetRandomInstrument(instruments));
            spawnedInstrument.transform.position = spawnPoint.transform.position;
        }
        */
    }

    private void OnDisable()
    {
        foreach (var grabbableInstrument in _grabbableInstruments)
        {
            Destroy(grabbableInstrument);
        }
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
