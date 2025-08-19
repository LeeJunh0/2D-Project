using System;
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
    public PlayerUnLockData playerFriendUnlock;

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
        playerFriendUnlock = new PlayerUnLockData();
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
    public int num;
    public string name;
    public float coinDefault;
    public float coinCoefficient;
    public float coinPerSec;

    public double Coin { get { return coinDefault * coinCoefficient; } }

    public StatInfo() { }
    public StatInfo(StatInfo info)
    {
        num = info.num;
        name = info.name;
        coinDefault = info.coinDefault;
        coinCoefficient = info.coinCoefficient;
        coinPerSec = info.coinPerSec;
    }
}

[System.Serializable]
public class FriendStatSet : ILoader<string, StatInfo>
{
    public List<StatInfo> FriendStatData { get; set; }

    public Dictionary<string, StatInfo> MakeDict()
    {
        Dictionary<string, StatInfo> dict = new Dictionary<string, StatInfo>();
        foreach (StatInfo info in FriendStatData)
            dict.Add(info.name, info);

        return dict;
    }
}

[System.Serializable]
public class NumberData
{
    public int number;
    public string name_desc;
}

[System.Serializable]
public class NumberDataSet : ILoader<int, NumberData>
{
    public List<NumberData> NumberData;
    public Dictionary<int, NumberData> MakeDict()
    {
        Dictionary<int, NumberData> dict = new Dictionary<int, NumberData>();
        foreach (NumberData info in NumberData)
            dict.Add(info.number, info);

        return dict;
    }
}

public enum UnlockObjectType
{
    None,
    Friend,
    Build,
    Item
}
public enum UnlockActionType
{
    None,
    Buy,
    Sell,
}

[System.Serializable]
public class UnLockData
{
    // 나중에 배열로 만들어서 여러개의 조건을 걸수도..?
    public UnlockObjectType objectType;
    public UnlockActionType actionType;
    public int objectNum;
    public int actionCount;
    public int curCount;
    public bool isCompleted;
    public int CurCount
    {
        get => curCount;
        set
        {
            curCount = Mathf.Clamp(value, 0,actionCount);

            if (curCount >= actionCount)
                isCompleted = true;
        }
    }
}

[System.Serializable]
public class FriendUnLockData
{
    public string objectName;
    public UnLockData unlockData;
}

[System.Serializable]
public class FriendUnLockDataSet : ILoader<string, FriendUnLockData>
{
    public List<FriendUnLockData> UnLockData { get; set; }
    public Dictionary<string, FriendUnLockData> MakeDict()
    {
        Dictionary<string, FriendUnLockData> dict = new Dictionary<string, FriendUnLockData>();
        foreach(FriendUnLockData data in  UnLockData)
            dict.Add(data.objectName, data);

        return dict;
    }
}

[System.Serializable]
public class FriendInfo
{
    public int number;
    public string objectName;
    public string friendIcon;
    public string name;
    public string description;
    public int price;
}

// 동적 능력치
[System.Serializable]
public class FriendStat
{
    public bool isEquip;
    public Define.EFriend_Rarity rarity = Define.EFriend_Rarity.Normal;
    public StatInfo info;

    public Define.EFriend_Rarity Rarity
    {
        get => rarity;
        set
        {
            rarity = value;
            switch (rarity)
            {
                case Define.EFriend_Rarity.Normal:
                    info.coinCoefficient = 1;
                    info.coinPerSec = 3.0f;
                    break;
                case Define.EFriend_Rarity.Rare:
                    info.coinCoefficient = 3f;
                    info.coinPerSec = 6.93f;
                    break;
                case Define.EFriend_Rarity.Named:
                    info.coinCoefficient = 7f;
                    info.coinPerSec = 14f;
                    break;
                case Define.EFriend_Rarity.Boss:
                    info.coinCoefficient = 12f;
                    info.coinPerSec = 18f;
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
        foreach (string friend in MainManager.Data.FriendStatDict.Keys)
            collectionDict.Add(friend, new FriendCollectionInfo());

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
public class PlayerUnLockData
{
    public Dictionary<string, UnLockData> unlockData;

    public void UnLockDataInit()
    {
        unlockData = new Dictionary<string, UnLockData>();
        foreach (FriendUnLockData unlock in MainManager.Data.FriendUnLockDataDict.Values)
            unlockData.Add(MainManager.Data.NumberDataDict[unlock.unlockData.objectNum].name_desc, unlock.unlockData);
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

    public bool CheckCollection(Define.EFriend_Rarity rarity)
    {
        if (rarity == Define.EFriend_Rarity.Normal && hasNormal == true)
            return true;
        if (rarity == Define.EFriend_Rarity.Rare && hasRare == true)
            return true;
        if (rarity == Define.EFriend_Rarity.Named && hasNamed == true)
            return true;
        if (rarity == Define.EFriend_Rarity.Boss && hasBoss == true)
            return true;

        return false;
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
