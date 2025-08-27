using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendStatus : MonoBehaviour
{
    public static event Action<int> FriendWalkOrRestHandler;
    public static event Action<int> OnIndexUpdateHandler;
    public static event Func<int, bool> FriendSellHandler;

    [SerializeField] private GameObject portrailBackGround;
    [SerializeField] private GameObject statusBackGround;

    [SerializeField] private TextMeshProUGUI friendCountText;
    [SerializeField] private Transform listContent;

    [SerializeField] private Animator renderTexture;
    [SerializeField] private TextMeshProUGUI portrailNameText;
    [SerializeField] private TextMeshProUGUI createCoinText;
    [SerializeField] private TextMeshProUGUI friendDesriptionText;
    [SerializeField] private TextMeshProUGUI friendRankText;

    [SerializeField] private Button sellButton;
    [SerializeField] private GameObject wanningScreen;
    [SerializeField] private Button reallySellButton;
    [SerializeField] private Button exitButton;

    private List<UI_FriendListSlot> friendListSlots;
    private UI_FriendListSlot curFriend;

    public void Start()
    {
        friendListSlots = new List<UI_FriendListSlot>();

        UI_Game.StatusOpenHandler -= SetFrieldStatus;
        UI_Game.StatusOpenHandler += SetFrieldStatus;
        EventManager.OnFriendCountUpdateHandler -= FriendCountUpdate;
        EventManager.OnFriendCountUpdateHandler += FriendCountUpdate;

        sellButton.gameObject.AddEvent(SellUIOpen);
        reallySellButton.gameObject.AddEvent(ReallySell);
        exitButton.gameObject.AddEvent(SellUIExit);
    }

    private void SetFrieldStatus(bool isOpen)
    {
        if (isOpen == false)
            return;

        FriendListInit(PlayerDataManager.Instance.FriendList);
        FriendStatusUpdate();
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
        FriendListEquipUpdate();
    }

    private void FriendListClear()
    {
        if (friendListSlots.Count <= 0)
            return;

        foreach (var slot in friendListSlots)
            Destroy(slot.gameObject);

        friendListSlots.Clear();
    }

    private void FriendListEquipUpdate()
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

    private void FriendStatusUpdate()
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
        createCoinText.text = string.Format($"생산량: {curFriendStat.info.coinPerSec}초당 {curFriendStat.info.Coin}원");
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
            case Define.EFriend_Rarity.Named:
                return "<color=green>네임드</color>";
            case Define.EFriend_Rarity.Boss:
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
        FriendStatusUpdate();
    }

    private void SellUIOpen(PointerEventData eventData)
    {
        wanningScreen.SetActive(true);
    }

    private void ReallySell(PointerEventData eventData)
    {
        if (FriendSellHandler == null)
            return;

        bool isComplete = FriendSellHandler.Invoke(curFriend.Index);
        if (isComplete == false)
            return;

        int removeIndex = curFriend.Index;
        friendListSlots.RemoveAt(removeIndex);
        OnIndexUpdateHandler?.Invoke(removeIndex);
        Destroy(curFriend.gameObject);
        curFriend = null;
        FriendStatusUpdate();
        SellUIExit(eventData);
    }

    private void SellUIExit(PointerEventData eventData)
    {
        wanningScreen.SetActive(false);
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
        UI_Game.StatusOpenHandler -= SetFrieldStatus;
        EventManager.OnFriendCountUpdateHandler -= FriendCountUpdate;

        sellButton.gameObject.RemoveEvent(SellUIOpen);
        reallySellButton.gameObject.RemoveEvent(ReallySell);
        exitButton.gameObject.RemoveEvent(SellUIExit);
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
