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
    public Dictionary<string, List<StatInfo>> MonsterLevelDict { get; private set; } = new Dictionary<string, List<StatInfo>>();
    public Dictionary<string, MonsterInfo> MonsterDataDict { get; private set; } = new Dictionary<string, MonsterInfo>();

    public void Init()
    {
        MonsterLevelDict = PasingJsonData<StatInfo>(JsonLoad("2D_Project_MonsterlevelData"));
        MonsterDataDict = PasingMonsterUIJsonData(JsonLoad("2D_Project_MonsterData"));
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

    private Dictionary<string, MonsterInfo> PasingMonsterUIJsonData(string json)
    {
        MonsterSet list = JsonUtility.FromJson<MonsterSet>(json);
        Dictionary<string, MonsterInfo> dict = new Dictionary<string, MonsterInfo>();

        foreach (MonsterInfo data in list.MonsterData)
            dict.Add(data.objectName, data);

        return dict;
    }
}
