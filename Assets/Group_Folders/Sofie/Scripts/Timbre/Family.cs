using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Family : MonoBehaviour
{
    public List<GameObject> instruments;

}

public enum FamilyType
{
    String,
    Woodwind,
    Brass,
    Percussion,
    Keyboard,
    Voice
}
