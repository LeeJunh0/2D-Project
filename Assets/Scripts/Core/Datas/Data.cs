using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public bool isFirst;
    public double gold;
    public Dictionary<string, List<MonsterStat>> monsters;
    public Dictionary<string, List<SerializableVector3>> mobPosDict;
    public Dictionary<string, List<BuildInfo>> builds;
    public Dictionary<string, List<SerializableVector3>> buildingPosDict;
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

// 정적 능력치
[System.Serializable]
public class MonsterStaticStat
{
    public string name;
    public int level;
    
    public float coinDefault;
    public float coinCoefficient;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            coinDefault = MainManager.Data.MonsterLevelDict[name][level].coinDefault;
            coinCoefficient = MainManager.Data.MonsterLevelDict[name][level].coinCoefficient;
        }
    }
}

// 동적 능력치
[System.Serializable]
public class MonsterStat
{
    public MonsterStaticStat info;
    public Define.EMonster_Rarity rarity = Define.EMonster_Rarity.Normal;

    public Define.EMonster_Rarity Rarity
    {
        get => rarity;
        set
        {
            rarity = value;
            switch (rarity)
            {
                case Define.EMonster_Rarity.None:
                    Rarity = Define.EMonster_Rarity.Normal;
                    break;
                case Define.EMonster_Rarity.Normal:
                    info.coinDefault *= 1;
                    info.coinCoefficient *= 1;
                    break;
                case Define.EMonster_Rarity.Rare:
                    info.coinDefault *= 1.15f;
                    info.coinCoefficient *= 1.15f;
                    break;
                case Define.EMonster_Rarity.Unique:
                    info.coinDefault *= 1.3f;
                    info.coinCoefficient *= 1.3f;
                    break;
                case Define.EMonster_Rarity.Legend:
                    info.coinDefault *= 2f;
                    info.coinCoefficient *= 2f;
                    break;
            }
        }
    }
}

[System.Serializable]
public class BuildInfo
{
    public string name;
    public string description;
    public int level;
    public int price;
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
