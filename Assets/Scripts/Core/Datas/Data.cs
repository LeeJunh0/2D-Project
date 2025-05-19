using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public double gold;
    public Dictionary<string, MonsterStat> monsters;
    public Dictionary<string, SerializableVector3> posDict;
}

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
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
