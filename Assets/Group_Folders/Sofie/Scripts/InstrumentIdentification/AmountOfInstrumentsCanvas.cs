using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmountOfInstrumentsCanvas : MonoBehaviour
{
   [SerializeField] private GameObject[] options;
    
    public void SetOptions(int optionAmount)
    { ;
        for (int i = 0; i < optionAmount; i++)
        {
            options[i].SetActive(true);
        }
    }
}
