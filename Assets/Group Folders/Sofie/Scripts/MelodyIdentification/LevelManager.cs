using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int level = 1;

    public AudioClip levelMelody { get; private set; }

    [SerializeField]
    private AudioClip[] melodies;

    [SerializeField]
    private GameObject melodyPrefab;
    private List<GameObject> instantiatedMelodies;


    private void Start()
    {
        StartLevel();
    }

    private int SetNumberofVariations(int level)
    {
        switch (level)
        {
            case 1:
                return 2;
            default:
                return 2;
        }
    }

    private AudioClip SetLevelMelody()
    {
        int rnd = Random.Range(0, melodies.Length);

        return melodies[rnd];
    }

    public void InstantiateMelodies(int variations)
    {
        for (int i = 0; i < variations; i++)
        {

            GameObject melody = Instantiate(melodyPrefab, new Vector3(i, 0, 0), Quaternion.identity);
            instantiatedMelodies.Add(melody);

            switch (i)
            {
                case 0:
                    melody.GetComponent<ButtonVariation>().key = KeyCode.E;
                    melody.GetComponent<ButtonVariation>().isIdentical = true;
                    break;
                case 1:
                    melody.GetComponent<ButtonVariation>().key = KeyCode.F;
                    break;
                case 2:
                    melody.GetComponent<ButtonVariation>().key = KeyCode.G;
                    break;
            }
        }
    }
    public void StartLevel()
    {
        levelMelody = SetLevelMelody();
        int variations = SetNumberofVariations(level);
        InstantiateMelodies(variations);
    }

    public void DeleteMelodies()
    {
        foreach(GameObject melody in instantiatedMelodies)
        {
            Destroy(melody);
        }
        instantiatedMelodies.Clear();
    }
}
