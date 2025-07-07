using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using TMPro;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [Header("플레이어 저장 데이터")]
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private List<Monster> monsterList;
    [SerializeField] private List<BaseBuilding> buildList;
    [SerializeField] private TextMeshProUGUI goldText;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject homePrefab;

    public double Gold { get { return playerInfo.gold; } set { playerInfo.gold = value; } }
    public bool IsLoadCompleted { get; set; }

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
        if (File.Exists(path) == false)
        {
            Extension.ErrorLog("Not Find Data");
            CreateNewData();
        }

        string jsonData = File.ReadAllText(path);
        playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(jsonData);
        Extension.SuccessLog("PlayerData Load Complete");

        monsterList = new List<Monster>();
        buildList = new List<BaseBuilding>();

        LoadMonsters();
        LoadBuilding();
    }

    private void CreateNewData()
    {
        // 새로만들 플레이어정보
        playerInfo = new PlayerInfo();
        playerInfo.isFirst = true;
        playerInfo.gold = 0;
        playerInfo.monsters = new Dictionary<string, List<MonsterStat>>();
        playerInfo.mobPosDict = new Dictionary<string, List<SerializableVector3>>();
        playerInfo.builds = new Dictionary<string, List<BuildInfo>>();
        playerInfo.buildingPosDict = new Dictionary<string, List<SerializableVector3>>();

        // 처음 있어야 할 건물
        GameObject home = Instantiate(homePrefab, parent);
        SerializableVector3 housePos = new SerializableVector3(home.transform.position);
        buildList.Add(home.FindChild<BaseBuilding>());
        playerInfo.builds.Add("House", new List<BuildInfo>() { buildList[0].Info });
        playerInfo.buildingPosDict.Add("House", new List<SerializableVector3>() { housePos });

        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Close();
        SaveData();
        Extension.SuccessLog("New Data Create Complete");
    }

    private void SaveData()
    {
        SaveMonsters();
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        string jsonData = JsonConvert.SerializeObject(playerInfo, Formatting.Indented);
        File.WriteAllText(path, jsonData);
        Extension.SuccessLog("Save Complete");
    }

    public void TextUpdate()
    {
        if (goldText == null)
            goldText = GameObject.Find("CoinText").GetOrAddComponent<TextMeshProUGUI>();

        goldText.text = ExChanger.GoldToText(playerInfo.gold);
    }

    // 저장된 몬스터 불러오기
    private void LoadMonsters()
    {
        foreach (KeyValuePair<string, List<MonsterStat>> info in playerInfo.monsters)
        {
            for (int i = 0; i < info.Value.Count; i++)
            {
                GameObject go = MainManager.Resource.Instantiate(info.Key);
                go.transform.position = playerInfo.mobPosDict[info.Key][i].ToVector3();

                Monster monster = go.GetComponent<Monster>();
                monster.Stat.info.name = monster.name;
                monster.Stat.info.Level = info.Value[i].info.level;

                monsterList.Add(monster);
            }
        }

        Extension.SuccessLog("몬스터 데이터 불러오기 완료");
    }

    // 이름으로 몬스터 만들기
    private void CreateMonster(string name)
    {
        GameObject go = MainManager.Resource.Instantiate(name);
        Monster monster = go.GetComponent<Monster>();
        monster.Stat.info.name = name;
        monster.Stat.info.Level = 1;
        monster.Stat.Rarity = MonsterGacha.RarityRandom();

        monsterList.Add(monster);
    }

    // 몬스터들 저장하기
    private void SaveMonsters()
    {
        playerInfo.monsters.Clear();
        playerInfo.mobPosDict.Clear();

        for (int i = 0; i < monsterList.Count; i++)
        {
            SerializableVector3 sbVec = new SerializableVector3(monsterList[i].transform.position);
            if(playerInfo.monsters.ContainsKey(monsterList[i].Stat.info.name) == true)
            {
                playerInfo.monsters[monsterList[i].Stat.info.name].Add(monsterList[i].Stat);
                playerInfo.mobPosDict[monsterList[i].Stat.info.name].Add(sbVec);
            }
            else
            {
                playerInfo.monsters.Add(monsterList[i].Stat.info.name, new List<MonsterStat>() { monsterList[i].Stat });
                playerInfo.mobPosDict.Add(monsterList[i].Stat.info.name, new List<SerializableVector3>() { sbVec });
            }
        }
    }

    // 저장된 건물 불러오기
    private void LoadBuilding()
    {
        foreach (var info in playerInfo.buildingPosDict)
        {
            for (int i = 0; i < info.Value.Count; i++)
            {
                GameObject go = MainManager.Resource.Instantiate(info.Key, parent);
                go.transform.position = info.Value[i].ToVector3();

                BaseBuilding build = go.GetComponent<BaseBuilding>();
                buildList.Add(build);
            }
        }

        Extension.SuccessLog("건물 데이터 불러오기 완료");
    }

    private void SaveBuilding()
    {
        playerInfo.builds.Clear();
        playerInfo.buildingPosDict.Clear();

        for (int i = 0; i < buildList.Count; i++)
        {
            SerializableVector3 sbVec = new SerializableVector3(buildList[i].transform.position);            
            if (playerInfo.builds.ContainsKey(buildList[i].Info.name) == true)
            {
                playerInfo.builds[buildList[i].Info.name].Add(buildList[i].Info);
                playerInfo.buildingPosDict[buildList[i].Info.name].Add(sbVec);
            }
            else
            {
                playerInfo.builds.Add(buildList[i].Info.name, new List<BuildInfo>() { buildList[i].Info });
                playerInfo.buildingPosDict.Add(buildList[i].Info.name, new List<SerializableVector3>() { sbVec });
            }
        }
    }
}
