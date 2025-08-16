using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 친구들의 능력치 관리 및 관련 UI관리
/// 나중에 커지게 되면 둘이 분리할수도..?
/// 
/// </summary>
public class FriendStatusController : Singleton<FriendStatusController>
{
    public static event Action<int> FriendWalkOrRestHandler;

    [SerializeField] private GameObject friendStatus;
    [SerializeField] private GameObject portrailBackGround;
    [SerializeField] private GameObject statusBackGround;

    [SerializeField] private TextMeshProUGUI friendCountText;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Transform listContent;

    [SerializeField] private Animator renderTexture;
    [SerializeField] private TextMeshProUGUI portrailNameText;
    [SerializeField] private TextMeshProUGUI createCoinText;
    [SerializeField] private TextMeshProUGUI friendDesriptionText;
    [SerializeField] private TextMeshProUGUI friendRankText;

    private List<UI_FriendListSlot> friendListSlots;
    private UI_FriendListSlot curFriend;

    public void Start()
    {
        friendListSlots = new List<UI_FriendListSlot>();
        exitButton.AddEvent(OffFriendStatus);
    }

    public void OnFrieldStatus(PointerEventData eventData)
    {
        friendStatus.SetActive(true);
        FriendListInit(PlayerDataManager.Instance.FriendList);
        FriendStatusInit();
    }

    private void OffFriendStatus(PointerEventData eventData)
    {
        friendStatus.SetActive(false);
    }

    public void FriendListInit(List<Friend> friends)
    {
        FriendListClear();

        UI_FriendListSlot.SelectFrinedCheckHandler -= SelectFriendListSlot;
        UI_FriendListSlot.SelectFrinedCheckHandler += SelectFriendListSlot;
        UI_FriendListSlot.WalkOrRestCheckHandler -= FriendWalkOrRest;
        UI_FriendListSlot.WalkOrRestCheckHandler += FriendWalkOrRest;

        for (int i = 0; i < friends.Count; i++)
        {
            GameObject go = MainManager.Resource.Instantiate("FriendList_Slot", listContent);
            UI_FriendListSlot slot = go.GetComponent<UI_FriendListSlot>();

            slot.Index = i;
            slot.Init(MainManager.Data.FriendDataDict[friends[i].Stat.info.name]);
            friendListSlots.Add(slot);
        }

        if (curFriend == null && friendListSlots.Count > 0)
            curFriend = friendListSlots[0];

        FriendCountUpdate();
        FriendListUpdate();
    }

    private void FriendListClear()
    {
        if (friendListSlots.Count <= 0)
            return;

        foreach (var slot in friendListSlots)
            Destroy(slot.gameObject);

        friendListSlots.Clear();
    }

    private void FriendListUpdate()
    {
        if (friendListSlots.Count <= 0)
            return;

        for (int i = 0; i < friendListSlots.Count; i++)
            friendListSlots[i].IsEquip = PlayerDataManager.Instance.FriendList[i].Stat.isEquip;
    }

    private void FriendCountUpdate()
    {
        friendCountText.text = string.Format($"{PlayerDataManager.Instance.CurFrieldCount} / {PlayerDataManager.Instance.MaxFriendCount}");
    }

    private void FriendStatusInit()
    {
        if (curFriend == null)
        {
            statusBackGround.SetActive(false);
            portrailBackGround.SetActive(false);
            return;
        }

        statusBackGround.SetActive(true);
        portrailBackGround.SetActive(true);
        FriendStat curFriendStat = PlayerDataManager.Instance.FriendList[curFriend.Index].Stat;
        renderTexture.runtimeAnimatorController = MainManager.Addressable.Load<RuntimeAnimatorController>($"Anim_{curFriendStat.info.name}");
        friendRankText.text = string.Format($"희귀도: {RarityToString(curFriendStat)}");
        portrailNameText.text = MainManager.Data.FriendDataDict[curFriendStat.info.name].name;
        createCoinText.text = string.Format($"생산량: {curFriendStat.info.coinDefault * curFriendStat.info.coinCoefficient}원");
        friendDesriptionText.text = MainManager.Data.FriendDataDict[curFriendStat.info.name].description;
    }

    private string RarityToString(FriendStat stat)
    {
        switch (stat.rarity)
        {
            case Define.EFriend_Rarity.Normal:
                return "<color=black>일반</color>";
            case Define.EFriend_Rarity.Rare:
                return "<color=blue>희귀</color>";
            case Define.EFriend_Rarity.Unique:
                return "<color=green>네임드</color>";
            case Define.EFriend_Rarity.Legend:
                return "<color=red>보스</color>";
            default:
                return "";
        }
    }

    private void SelectFriendListSlot(UI_FriendListSlot slot)
    {
        if (curFriend != null)
            curFriend.OffOutLine();

        curFriend = slot;
        curFriend.OnOutLine();
        FriendStatusInit();
    }

    private void FriendWalkOrRest(UI_FriendListSlot slot)
    {
        FriendWalkOrRestHandler?.Invoke(slot.Index);
        slot.IsEquip = PlayerDataManager.Instance.FriendList[slot.Index].Stat.isEquip;
        FriendCountUpdate();
    }

    private void HandlerClear()
    {
        UI_FriendListSlot.SelectFrinedCheckHandler -= SelectFriendListSlot;
        UI_FriendListSlot.WalkOrRestCheckHandler -= FriendWalkOrRest;
    }

    private void OnDestroy()
    {
        HandlerClear();
    }

    private void OnDisable()
    {
        HandlerClear();
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
