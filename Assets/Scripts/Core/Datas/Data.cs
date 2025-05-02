using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public double gold;
    public List<MonsterStat> monsters;
}

[System.Serializable]
public class StatInfo
{
    public int level;
    public float coinDefault;
    public float coinCoefficient;

    public double Coin { get { return coinDefault * coinCoefficient; } }
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
