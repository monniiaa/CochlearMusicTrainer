using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataContainerList<T> where T: AbstractDataContainer
{
    public List<T> DataList;
    public string ToJson() => JsonUtility.ToJson(this, true);
}
