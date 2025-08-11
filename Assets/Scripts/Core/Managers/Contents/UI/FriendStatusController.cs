using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 친구들의 능력치 관리 및 관련 UI관리
/// 나중에 커지게 되면 둘이 분리할수도..?
/// 
/// </summary>
public class FriendStatusController : Singleton<FriendStatusController>
{
    public static event Action SelectFrinedCheckHandler;

    [SerializeField] private GameObject friendStatus;

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

    public UI_FriendListSlot CurFriend
    {
        get => curFriend;
        set
        {
            if (curFriend != value)
                curFriend = value;
            else
                curFriend = null;

            SelectFrinedCheckHandler?.Invoke();
        }
    }

    public void Start()
    {
        friendListSlots = new List<UI_FriendListSlot>();
        exitButton.AddEvent(OffFriendStatus);
    }

    public void OnFrieldStatus(PointerEventData eventData)
    {
        friendStatus.SetActive(true);
        FriendListInit(PlayerDataManager.Instance.FriendList);
    }

    private void OffFriendStatus(PointerEventData eventData)
    {
        friendStatus.SetActive(false);
    }

    public void FriendListInit(List<Friend> friends)
    {
        FriendListClear();
        for (int i = 0; i < friends.Count; i++)
        {
            GameObject go = MainManager.Resource.Instantiate("FriendList_Slot", listContent);
            UI_FriendListSlot slot = go.GetComponent<UI_FriendListSlot>();

            slot.Index = i;
            slot.Init(MainManager.Data.FriendDataDict[friends[i].Stat.info.name]);
            friendListSlots.Add(slot);
        }

        if (curFriend == null || friendListSlots.Count > 0)
            CurFriend = friendListSlots[0];

        friendCountText.text = string.Format($"{PlayerDataManager.Instance.CurFrieldCount} / {PlayerDataManager.Instance.MaxFriendCount}");
        FriendListUpdate();
        FriendStatusInit();
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

    private void FriendStatusInit()
    {
        if (curFriend == null)
            return;

        FriendStat curFriendStat = PlayerDataManager.Instance.FriendList[curFriend.Index].Stat;
        renderTexture.runtimeAnimatorController = MainManager.Addressable.Load<RuntimeAnimatorController>($"Anim_{curFriendStat.info.name}");
        friendRankText.text = string.Format($"희귀도: {RarityToString(curFriendStat)}");
        portrailNameText.text = curFriendStat.info.name;
        createCoinText.text = string.Format($"기본값: {curFriendStat.info.coinDefault}, {curFriendStat.info.coinCoefficient}");
        //friendDesriptionText.text = MainManager.Data.FriendDataDict[]
    }

    private string RarityToString(FriendStat stat)
    {
        switch (stat.rarity)
        {
            case Define.EFriend_Rarity.Normal:
                return "일반";
            case Define.EFriend_Rarity.Rare:
                return "희귀";
            case Define.EFriend_Rarity.Unique:
                return "네임드";
            case Define.EFriend_Rarity.Legend:
                return "보스";
            default:
                return "";
        }
    }
}
