using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatInfo
{
    public float level;
    public float coinDefault;
    public float coinCoefficient;

    public float Coin { get { return coinDefault * (level * coinCoefficient); } }
}

[System.Serializable]
public class MonsterInfo
{
    public string objectName;
    public string name;
    public string description;
}

[System.Serializable]
public class MonsterSet
{
    public List<MonsterInfo> MonsterData;
}
