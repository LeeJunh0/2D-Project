using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public bool isFirst;
    public double gold;
    public Dictionary<string, List<FriendStat>> friends;
    public Dictionary<string, List<SerializableVector3>> friendPosDict;
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
public class FriendInfo
{
    public string objectName;
    public string friendIcon;
    public string name;
    public string description;
}

// 정적 능력치
[System.Serializable]
public class FriendStaticStat
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
            coinDefault = MainManager.Data.FriendLevelDict[name][level].coinDefault;
            coinCoefficient = MainManager.Data.FriendLevelDict[name][level].coinCoefficient;
        }
    }
}

// 동적 능력치
[System.Serializable]
public class FriendStat
{
    public FriendStaticStat info;
    public Define.EFriend_Rarity rarity = Define.EFriend_Rarity.Normal;

    public Define.EFriend_Rarity Rarity
    {
        get => rarity;
        set
        {
            rarity = value;
            switch (rarity)
            {
                case Define.EFriend_Rarity.None:
                    Rarity = Define.EFriend_Rarity.Normal;
                    break;
                case Define.EFriend_Rarity.Normal:
                    info.coinDefault *= 1;
                    info.coinCoefficient *= 1;
                    break;
                case Define.EFriend_Rarity.Rare:
                    info.coinDefault *= 1.15f;
                    info.coinCoefficient *= 1.15f;
                    break;
                case Define.EFriend_Rarity.Unique:
                    info.coinDefault *= 1.3f;
                    info.coinCoefficient *= 1.3f;
                    break;
                case Define.EFriend_Rarity.Legend:
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
    public string buildIcon;
    public string objectName;
}

[System.Serializable]
public class BuildSet : ILoader<string,  BuildInfo>
{
    public List<BuildInfo> BuildData { get; set; }
    public Dictionary<string, BuildInfo> MakeDict()
    {
        Dictionary<string,BuildInfo> dict = new Dictionary<string, BuildInfo>();
        foreach(BuildInfo info in BuildData)
            dict.Add(info.name, info);

        return dict;
    }
}

[System.Serializable]
public class FriendSet : ILoader<string, FriendInfo>
{
    public List<FriendInfo> FriendData { get; set; }

    public Dictionary<string, FriendInfo> MakeDict()
    {
        Dictionary<string, FriendInfo> dict = new Dictionary<string, FriendInfo>();
        foreach(FriendInfo friendInfo in FriendData)        
            dict.Add(friendInfo.name, friendInfo);

        return dict;
    }
}
