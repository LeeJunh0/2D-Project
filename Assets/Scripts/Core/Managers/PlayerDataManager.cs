using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    [Header("플레이어 저장 데이터")]
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private List<Monster> monsterList;


    [SerializeField] private TextMeshProUGUI goldText;

    public double Gold { get { return playerInfo.gold; }  set { playerInfo.gold = value; } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SaveData();

        if (Input.GetKeyDown(KeyCode.V))
            CreateMonster("AngryPig");
    }

    public void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        if(File.Exists(path) == false)
        {
            Debug.Log("Not Find Data");
            CreateNewData();
            Debug.Log("New Data Create Complete");
        }

        string jsonData = File.ReadAllText(path);
        playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(jsonData);
        Debug.Log("PlayerData Load Complete");

        monsterList = new List<Monster>();
        LoadMonsters();
    }

    private void CreateNewData()
    {
        playerInfo = new PlayerInfo();
        playerInfo.gold = 0;
        playerInfo.monsters = new Dictionary<string, MonsterStat>();
        playerInfo.posDict = new Dictionary<string, SerializableVector3>();

        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Close();
        SaveData();
    }

    private void SaveData()
    {
        SaveMonsters();
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        string jsonData = JsonConvert.SerializeObject(playerInfo, Formatting.Indented);
        File.WriteAllText(path, jsonData);
        Debug.Log("Save Complete");
        Debug.Log(jsonData);
    }

    public void TextUpdate()
    {
        if (goldText == null)
            goldText = GameObject.Find("CoinText").GetOrAddComponent<TextMeshProUGUI>();

        goldText.text = ExChanger.GoldToText(playerInfo.gold);
    }

    public void LoadMonsters()
    {
        foreach (KeyValuePair<string, MonsterStat> info in playerInfo.monsters) 
        {
            GameObject go = MainManager.Resource.Instantiate(info.Key);
            go.transform.position = playerInfo.posDict[info.Key].ToVector3();

            Monster monster = go.GetComponent<Monster>();
            monster.Stat.name = monster.name;
            monster.Stat.Level = info.Value.level;

            monsterList.Add(monster);
        }

        Debug.Log("Monster Load Complete");
    }

    public void CreateMonster(string name)
    {
        GameObject go = MainManager.Resource.Instantiate(name);
        Monster monster = go.GetComponent<Monster>();
        monster.Stat.name = name;
        monster.Stat.Level = 1;

        monsterList.Add(monster);
    }

    public void SaveMonsters()
    {
        for(int i = 0; i < monsterList.Count; i++)
        {
            SerializableVector3 sbVec = new SerializableVector3(monsterList[i].transform.position);
            playerInfo.monsters.Add(monsterList[i].Stat.Name, monsterList[i].Stat);
            playerInfo.posDict.Add(monsterList[i].Stat.Name, sbVec);
        }
    }
}
