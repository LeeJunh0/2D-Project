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
            EventManager.FriendCountUpdate();
        }
    }
    public int MaxFriendCount { get => playerInfo.maxFriendCount; } // set은 생각해보자 
    public bool IsLoadCompleted { get; set; }
    public List<Friend> FriendList => friendList;
    public PlayerCollection Collection => playerInfo.playerCollection;
    public PlayerUnLockData UnLockData => playerInfo.playerFriendUnlock;

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

            if (playerInfo.maxFriendCount <= 0)
            {
                Extension.ErrorLog("잘못된 세이브 데이터를 불러와서 초기화 합니다.");
                CreateNewData();
            }
            else
            {
                Extension.SuccessLog("PlayerData Load Complete");

                friendList = new List<Friend>();
                buildList = new List<BaseBuilding>();

                LoadFriends();
                LoadBuilding();
            }
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
        UI_FriendStatus.FriendWalkOrRestHandler -= FriendWalkOrRest;
        UI_FriendStatus.FriendWalkOrRestHandler += FriendWalkOrRest;
        UI_FriendStatus.FriendSellHandler -= SellFriend;
        UI_FriendStatus.FriendSellHandler += SellFriend;
        UI_FriendShopSlot.BuyFriendHandler -= BuyFriendGoldCheck;
        UI_FriendShopSlot.BuyFriendHandler += BuyFriendGoldCheck;

    }

    private void CreateNewData()
    {
        // 새로만들 플레이어정보
        playerInfo = new PlayerInfo();
        playerInfo.playerCollection.CollectionDictInit();
        playerInfo.playerFriendUnlock.UnLockDataInit();

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
                GameObject go = MainManager.Resource.Instantiate(info.Key);
                go.transform.position = playerInfo.friendPosDict[info.Key][i].ToVector3();
                Friend friend = go.GetComponent<Friend>();
                friend.Stat = info.Value[i];
                friendList.Add(friend);

                friend.gameObject.SetActive(friend.Stat.isEquip);
            }
        }

        Extension.SuccessLog("몬스터 데이터 불러오기 완료");
    }

    private void BuyFriendGoldCheck(string name)
    {
        if (playerInfo.gold < MainManager.Data.FriendDataDict[name].price)
        {
            Extension.ErrorLog("넌 돈이 없는 거지로구나...으음");
            return;
        }

        if (MaxFriendCount <= CurFrieldCount)
        {
            Extension.ErrorLog("최대 친구수입니다.");
            return;
        }

        playerInfo.gold -= MainManager.Data.FriendDataDict[name].price;
        EventManager.UnLockActionBuy(name);
        GoldUpdate();
        CreateFriend(name); //TODO : 가챠UI 만들기
    }

    private bool SellFriend(int index)
    {
        if (friendList.Count < index)
        {
            Extension.ErrorLog("말도안되는 접근입니다.");
            return false;
        }

        if (friendList[index] == null)
        {
            Extension.ErrorLog("해당 인덱스의 데이터는 null 입니다.");
            return false;
        }

        playerInfo.gold += MainManager.Data.FriendDataDict[friendList[index].Stat.info.name].price / 3;
        EventManager.UnLockActionSell(index);
        Destroy(FriendList[index].gameObject);
        FriendList.RemoveAt(index);
        CurFrieldCount--;
        GoldUpdate();
        Extension.SuccessLog("판매성공 확인바람");
        return true;
    }

    // 이름으로 친구 만들기
    private void CreateFriend(string name)
    {
        if (CurFrieldCount >= MaxFriendCount)
        {
            Extension.ErrorLog("친구 수가 최대치입니다.");
            return;
        }

        GameObject go = MainManager.Resource.Instantiate(name);
        Friend friend = go.GetComponent<Friend>();
        Define.EFriend_Rarity rarity = FriendGacha.RarityRandom();
        friend.Stat.info = new StatInfo(MainManager.Data.FriendStatDict[name]);
        CurFrieldCount++;
        friend.Stat.Rarity = rarity;
        friend.Stat.isEquip = true;
        friendList.Add(friend);
        go.SetActive(true);

        playerInfo.playerCollection.GetCollection(name, rarity);
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
        friendList[index].gameObject.SetActive(friendList[index].Stat.isEquip);
    }

    private void HandlerClear()
    {
        UI_FriendStatus.FriendWalkOrRestHandler -= FriendWalkOrRest;
        UI_FriendShopSlot.BuyFriendHandler -= BuyFriendGoldCheck;
        UI_FriendStatus.FriendSellHandler -= SellFriend;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
