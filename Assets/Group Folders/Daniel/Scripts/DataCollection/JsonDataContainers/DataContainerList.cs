using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataContainerList<T> where T: AbstractDataContainer
{
    public List<T> DataList = new List<T>();
    public string ToJson() => JsonUtility.ToJson(this, false);
}
