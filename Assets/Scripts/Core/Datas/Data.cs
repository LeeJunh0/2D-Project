using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public string name;
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
public class MonsterSet : ILoader<string, MonsterInfo>
{
    public List<MonsterInfo> MonsterData { get; set; }

    public Dictionary<string, MonsterInfo> MakeDict()
    {
        Dictionary<string, MonsterInfo> dict = new Dictionary<string, MonsterInfo>();

        foreach(MonsterInfo monsterInfo in MonsterData)        
            dict.Add(monsterInfo.name, monsterInfo);

        return dict;
    }
}
