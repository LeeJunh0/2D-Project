using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public interface ILoader<Key, Value>
{
    public Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, StatInfo> FriendStatDict { get; private set; } = new Dictionary<string, StatInfo>();
    public Dictionary<string, FriendInfo> FriendDataDict { get; private set; } = new Dictionary<string, FriendInfo>();
    public Dictionary<string, FriendUnLockData> FriendUnLockDataDict { get; private set; } = new Dictionary<string, FriendUnLockData>();
    public Dictionary<int, NumberData> NumberDataDict { get; private set; } = new Dictionary<int, NumberData>();
    public Dictionary<string, BuildInfo> BuildDataDict { get; private set; } = new Dictionary<string, BuildInfo>();

    public void Init()
    {
        FriendStatDict = LoadData<string, StatInfo, FriendStatSet>("2D_Project_FriendlStatData");
        FriendDataDict = LoadData<string, FriendInfo, FriendSet>("2D_Project_FriendData");
        FriendUnLockDataDict = LoadData<string, FriendUnLockData, FriendUnLockDataSet>("2D_Project_FriendUnLockData");
        NumberDataDict = LoadData<int, NumberData, NumberDataSet>("2D_Project_NumberData");
        BuildDataDict = LoadData<string, BuildInfo, BuildSet>("2D_Project_BuildData");
    }

    private T LoadJson<T>(string path)
    {
        TextAsset json = MainManager.Addressable.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<T>(json.text);
    }

    private Dictionary<Tkey, TValue> LoadData<Tkey, TValue, TLoader>(string path) where TLoader : ILoader<Tkey, TValue>
    {
        TLoader data = LoadJson<TLoader>(path);
        return data.MakeDict();
    }
}
