using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_FriendShop : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private UI_UnLockToolTip unlockTip;

    private Dictionary<string, UI_FriendShopSlot> dictSlots;

    private void Start()
    {
        UI_Game.ShopOpenHandler -= SetFriendShop;
        UI_Game.ShopOpenHandler += SetFriendShop;
        EventManager.OnUnLockSlotHandler -= SlotUnLock;
        EventManager.OnUnLockSlotHandler += SlotUnLock;

        dictSlots = new Dictionary<string, UI_FriendShopSlot>();
    }

    private void Init()
    {
        DictClear();
        foreach (var info in PlayerDataManager.Instance.UnLockData.unlockData)
        {
            if (info.Key == "None")
                continue;

            GameObject go = MainManager.Resource.Instantiate("FriendShop_Slot", content);
            UI_FriendShopSlot slot = go.GetComponent<UI_FriendShopSlot>();

            slot.Init(info.Key);
            slot.SetUnLock(info.Value.isUnLock);
            dictSlots.Add(info.Key, slot);
        }

        UI_FriendShopSlot.EnterSlotHandler -= OnUnLockTip;
        UI_FriendShopSlot.EnterSlotHandler += OnUnLockTip;
        UI_FriendShopSlot.ExitSlotHandler -= OffUnLockTip;
        UI_FriendShopSlot.ExitSlotHandler += OffUnLockTip;
    }

    private void SlotUnLock(string name)
    {
        if (dictSlots.ContainsKey(name) == false)
            return;

        UnLockData data = PlayerDataManager.Instance.UnLockData.unlockData[name];
        dictSlots[name].SetUnLock(data.isUnLock);
    }

    private void SetFriendShop(bool isOpen)
    {
        if (isOpen == false)
            return;

        if (dictSlots.Count <= 0 )
            Init();
    }

    private void OnUnLockTip(string name)
    {
        if (MainManager.Data.FriendUnLockDataDict.ContainsKey(name) == false)
            return;

        unlockTip.gameObject.SetActive(true);
        unlockTip.Init(PlayerDataManager.Instance.UnLockData.unlockData[name]);
    }

    private void OffUnLockTip()
    {
        unlockTip.gameObject.SetActive(false);
    }

    private void DictClear()
    {
        if (dictSlots.Count <= 0)
            return;

        foreach (var slot in dictSlots.Values)
            Destroy(slot.gameObject);

        dictSlots.Clear();
    }

    private void HandlerClear()
    {
        UI_Game.ShopOpenHandler -= SetFriendShop;
        UI_FriendShopSlot.EnterSlotHandler -= OnUnLockTip;
        UI_FriendShopSlot.ExitSlotHandler -= OffUnLockTip;
        EventManager.OnUnLockSlotHandler -= SlotUnLock;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
