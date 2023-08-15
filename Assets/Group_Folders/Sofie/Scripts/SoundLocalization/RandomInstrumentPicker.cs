using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstrumentPicker : MonoBehaviour
{
    [SerializeField] private GameObject[] instruments;
    // Start is called before the first frame update

    public List<GameObject> PickedInstruments(int amount)
    {
        List<GameObject> pickedInstruments = new List<GameObject>();
        int rand;
        for (int i = 0; i < amount; i++)
        {
            do
            {
                rand = Random.Range(0, instruments.Length);
            } while (pickedInstruments.Contains(instruments[rand]));
            pickedInstruments.Add(instruments[rand]);
        }
        return pickedInstruments;
    }
}
