using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public int maxFriendCount;
    public int curFriendCount;
    public bool isFirst;
    public double gold;
    public Dictionary<string, List<FriendStat>> friends;
    public Dictionary<string, List<SerializableVector3>> friendPosDict;
    public Dictionary<string, List<BuildInfo>> builds;
    public Dictionary<string, List<SerializableVector3>> buildingPosDict;
    public PlayerCollection playerCollection;

    public PlayerInfo()
    {
        maxFriendCount = 5;
        curFriendCount = 0;
        isFirst = true;
        gold = 0;
        friends = new Dictionary<string, List<FriendStat>>();
        friendPosDict = new Dictionary<string, List<SerializableVector3>>();
        builds = new Dictionary<string, List<BuildInfo>>();
        buildingPosDict = new Dictionary<string, List<SerializableVector3>>();
        playerCollection = new PlayerCollection();
    }
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
    public bool isEquip;
    public Define.EFriend_Rarity rarity = Define.EFriend_Rarity.Normal;

    public Define.EFriend_Rarity Rarity
    {
        get => rarity;
        set
        {
            rarity = value;
            switch (rarity)
            {
                case Define.EFriend_Rarity.Normal:
                    info.coinDefault *= 1;
                    info.coinCoefficient *= 1;
                    break;
                case Define.EFriend_Rarity.Rare:
                    info.coinDefault *= 1.15f;
                    info.coinCoefficient *= 1.15f;
                    break;
                case Define.EFriend_Rarity.Named:
                    info.coinDefault *= 1.3f;
                    info.coinCoefficient *= 1.3f;
                    break;
                case Define.EFriend_Rarity.Boss:
                    info.coinDefault *= 2f;
                    info.coinCoefficient *= 2f;
                    break;
            }
        }
    }
}

[System.Serializable]
public class PlayerCollection
{
    public int totalFriendCount;
    public int hasFriendCount;
    public Dictionary<string, FriendCollectionInfo> collectionDict;

    public void CollectionDictInit()
    {
        collectionDict = new Dictionary<string, FriendCollectionInfo>();
        foreach (FriendInfo friend in MainManager.Data.FriendDataDict.Values)
            collectionDict.Add(friend.objectName, new FriendCollectionInfo());

        totalFriendCount = collectionDict.Count * 4;
    }

    public void GetCollection(string name, Define.EFriend_Rarity rarity)
    {
        if (rarity == Define.EFriend_Rarity.Normal && collectionDict[name].hasNormal == true)
            return;
        if (rarity == Define.EFriend_Rarity.Rare && collectionDict[name].hasRare == true)
            return;
        if (rarity == Define.EFriend_Rarity.Named && collectionDict[name].hasNamed == true)
            return;
        if (rarity == Define.EFriend_Rarity.Boss && collectionDict[name].hasBoss == true)
            return;

        collectionDict[name].GetCollection(rarity);
        hasFriendCount = Mathf.Clamp(hasFriendCount + 1, 0, totalFriendCount);
    }
}

[System.Serializable]
public class FriendCollectionInfo
{
    public bool hasNormal;
    public bool hasRare;
    public bool hasNamed;
    public bool hasBoss;

    public FriendCollectionInfo()
    {
        hasNormal = false;
        hasRare = false;
        hasNamed = false;
        hasBoss = false;
    }

    public void GetCollection(Define.EFriend_Rarity rarity)
    {
        switch (rarity)
        {
            case Define.EFriend_Rarity.Normal:
                hasNormal = true;
                break;
            case Define.EFriend_Rarity.Rare:
                hasRare = true;
                break;
            case Define.EFriend_Rarity.Named:
                hasNamed = true;
                break;
            case Define.EFriend_Rarity.Boss:
                hasBoss = true;
                break;
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
public class BuildSet : ILoader<string, BuildInfo>
{
    public List<BuildInfo> BuildData { get; set; }
    public Dictionary<string, BuildInfo> MakeDict()
    {
        Dictionary<string, BuildInfo> dict = new Dictionary<string, BuildInfo>();
        foreach (BuildInfo info in BuildData)
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
        foreach (FriendInfo friendInfo in FriendData)
            dict.Add(friendInfo.objectName, friendInfo);

        return dict;
    }
}
