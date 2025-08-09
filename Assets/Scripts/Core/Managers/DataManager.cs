using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public interface ILoader<Key, Value>
{
    public Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, List<StatInfo>> FriendLevelDict { get; private set; } = new Dictionary<string, List<StatInfo>>();
    public Dictionary<string, FriendInfo> FriendDataDict { get; private set; } = new Dictionary<string, FriendInfo>();
    public Dictionary<string, BuildInfo> BuildDataDict { get; private set; } = new Dictionary<string, BuildInfo>();

    public void Init()
    {
        FriendLevelDict = PasingJsonData<StatInfo>(JsonLoad("2D_Project_FriendlevelData"));
        FriendDataDict = LoadData<string, FriendInfo, FriendSet>("2D_Project_FriendData");
        BuildDataDict = LoadData<string, BuildInfo, BuildSet>("2D_Project_BuildData");
    }

    public string JsonLoad(string path)
    {
        TextAsset json = MainManager.Addressable.Load<TextAsset>(path);
        return json.text;
    }

    private Dictionary<string, List<T>> PasingJsonData<T>(string json) where T : StatInfo
    {
        return JsonConvert.DeserializeObject<Dictionary<string, List<T>>>(json);
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
