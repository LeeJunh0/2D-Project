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
    [SerializeField] private List<Friend> friendList;
    [SerializeField] private List<BaseBuilding> buildList;
    [SerializeField] private TextMeshProUGUI goldText;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject homePrefab;

    public double Gold { get => playerInfo.gold; set { playerInfo.gold = value; } }
    public int CurFrieldCount
    {
        get => playerInfo.curFriendCount;
        set
        {
            playerInfo.curFriendCount = Mathf.Clamp(value, 0, playerInfo.maxFriendCount);
        }
    }
    public int MaxFriendCount { get => playerInfo.maxFriendCount; } // set은 생각해보자 
    public bool IsLoadCompleted { get; set; }
    public List<Friend> FriendList => friendList;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CreateFriend("AngryPig");
    }

    public void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        if (File.Exists(path) == false)
        {
            Extension.ErrorLog("Not Find Data");
            CreateNewData();
        }
        else
        {
            string jsonData = File.ReadAllText(path);
            playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(jsonData);
            Extension.SuccessLog("PlayerData Load Complete");

            friendList = new List<Friend>();
            buildList = new List<BaseBuilding>();

            LoadFriends();
            LoadBuilding();
        }

        foreach (var build in buildList)
        {
            if (build.Info.objectName == "House")
            {
                goldText = build.GetComponentInChildren<TextMeshProUGUI>();
                break;
            }
        }

        GoldUpdate();
        FriendStatusController.FriendWalkOrRestHandler -= FriendWalkOrRest;
        FriendStatusController.FriendWalkOrRestHandler += FriendWalkOrRest;
    }

    private void CreateNewData()
    {
        // 새로만들 플레이어정보
        playerInfo = new PlayerInfo();

        // 처음 있어야 할 건물
        GameObject home = Instantiate(homePrefab, BuildingManager.Instance.BuildParent);
        SerializableVector3 housePos = new SerializableVector3(home.transform.position);
        BaseBuilding baseBuilding = home.FindChild<BaseBuilding>();
        baseBuilding.Info = MainManager.Data.BuildDataDict["집"];
        buildList.Add(baseBuilding);
        AddBuild(baseBuilding);

        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Close();
        SaveData();
        Extension.SuccessLog("New Data Create Complete");
    }

    public void SaveData()
    {
        SaveFriends();
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        string jsonData = JsonConvert.SerializeObject(playerInfo, Formatting.Indented);
        File.WriteAllText(path, jsonData);
        Extension.SuccessLog("Save Complete");
    }

    public void GoldUpdate()
    {
        if (goldText == null)
        {
            Extension.ErrorLog("goldText가 null입니다.");
            return;
        }

        goldText.text = ExChanger.GoldToText(playerInfo.gold);
    }

    // 저장된 친구 불러오기
    private void LoadFriends()
    {
        foreach (KeyValuePair<string, List<FriendStat>> info in playerInfo.friends)
        {
            for (int i = 0; i < info.Value.Count; i++)
            {
                if (info.Value[i].isEquip == false)
                    return;

                GameObject go = MainManager.Resource.Instantiate(info.Key);
                go.transform.position = playerInfo.friendPosDict[info.Key][i].ToVector3();
                Friend friend = go.GetComponent<Friend>();
                friend.Stat = info.Value[i];
                CurFrieldCount++;
                friendList.Add(friend);
            }
        }

        Extension.SuccessLog("몬스터 데이터 불러오기 완료");
    }

    // 이름으로 친구 만들기
    private void CreateFriend(string name)
    {
        GameObject go;
        Friend friend;
        if (CurFrieldCount >= MaxFriendCount)
        {
            go = MainManager.Addressable.Load<GameObject>(name);
            friend = go.GetComponent<Friend>();
            friend.Stat.info.name = name;
            friend.Stat.info.Level = 1;
            CurFrieldCount++;
            friend.Stat.Rarity = FriendGacha.RarityRandom();
            friend.Stat.isEquip = false;
            friendList.Add(friend);
        }
        else
        {
            go = MainManager.Resource.Instantiate(name);
            friend = go.GetComponent<Friend>();
            friend.Stat.info.name = name;
            friend.Stat.info.Level = 1;
            CurFrieldCount++;
            friend.Stat.Rarity = FriendGacha.RarityRandom();
            friend.Stat.isEquip = true;
            friendList.Add(friend);
        }

        AddFriend(friend);
    }

    // 친구들 저장하기
    private void SaveFriends()
    {
        playerInfo.friends.Clear();
        playerInfo.friendPosDict.Clear();

        for (int i = 0; i < friendList.Count; i++)
            AddFriend(friendList[i]);
    }

    // 저장된 건물 불러오기
    private void LoadBuilding()
    {
        foreach (var info in playerInfo.buildingPosDict)
        {
            for (int i = 0; i < info.Value.Count; i++)
            {
                GameObject go = MainManager.Resource.Instantiate(info.Key, BuildingManager.Instance.BuildParent);
                go.transform.position = info.Value[i].ToVector3();

                BaseBuilding build = go.GetComponent<BaseBuilding>();
                build.Info = playerInfo.builds[info.Key][i];
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
            AddBuild(buildList[i]);
    }

    public void AddFriend(Friend friend)
    {
        SerializableVector3 sbVec = new SerializableVector3(friend.transform.position);
        if (playerInfo.friends.ContainsKey(friend.Stat.info.name) == true)
        {
            playerInfo.friends[friend.Stat.info.name].Add(friend.Stat);
            playerInfo.friendPosDict[friend.Stat.info.name].Add(sbVec);
        }
        else
        {
            playerInfo.friends.Add(friend.Stat.info.name, new List<FriendStat>() { friend.Stat });
            playerInfo.friendPosDict.Add(friend.Stat.info.name, new List<SerializableVector3>() { sbVec });
        }
    }

    public void AddBuild(BaseBuilding build)
    {
        SerializableVector3 sbVec = new SerializableVector3(build.transform.position);
        if (playerInfo.builds.ContainsKey(build.Info.objectName) == true)
        {
            playerInfo.builds[build.Info.objectName].Add(build.Info);
            playerInfo.buildingPosDict[build.Info.objectName].Add(sbVec);
        }
        else
        {
            playerInfo.builds.Add(build.Info.objectName, new List<BuildInfo>() { build.Info });
            playerInfo.buildingPosDict.Add(build.Info.objectName, new List<SerializableVector3>() { sbVec });
        }
    }

    private void FriendWalkOrRest(int index)
    {
        friendList[index].Stat.isEquip = !friendList[index].Stat.isEquip;

        if (friendList[index].Stat.isEquip == true)
            CurFrieldCount++;
        else
            CurFrieldCount--;
    }

    private void OnApplicationQuit()
    {
        FriendStatusController.FriendWalkOrRestHandler -= FriendWalkOrRest;
    }
}
