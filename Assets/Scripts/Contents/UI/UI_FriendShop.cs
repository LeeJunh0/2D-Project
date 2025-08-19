using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendShop : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private Button exitButton;
    [SerializeField] private Transform content;
    [SerializeField] private UI_UnLockToolTip unlockTip;

    private List<UI_FriendShopSlot> listSlots;

    private void Start()
    {
        UI_Game.ShopOpenHandler -= OnFriendShop;
        UI_Game.ShopOpenHandler += OnFriendShop;
        listSlots = new List<UI_FriendShopSlot>();
    }

    private void Init()
    {
        ListClear();
        foreach(var info in MainManager.Data.FriendStatDict)
        {
            GameObject go = MainManager.Resource.Instantiate("FriendShop_Slot", content);
            UI_FriendShopSlot slot = go.GetComponent<UI_FriendShopSlot>();
            slot.Init(info.Key);
            listSlots.Add(slot);
        }

        exitButton.gameObject.AddEvent(OffFriendShop);

        UI_FriendShopSlot.EnterSlotHandler -= OnUnLockTip;
        UI_FriendShopSlot.EnterSlotHandler += OnUnLockTip;
        UI_FriendShopSlot.ExitSlotHandler -= OffUnLockTip;
        UI_FriendShopSlot.ExitSlotHandler += OffUnLockTip;
    }

    private void OnFriendShop()
    {
        ui.SetActive(true);
        Init();
    }
    private void OffFriendShop(PointerEventData eventData)
    {
        ui.SetActive(false);
    }

    private void OnUnLockTip(string name)
    {
        if (MainManager.Data.FriendUnLockDataDict.ContainsKey(name) == false)
            return;

        unlockTip.gameObject.SetActive(true);
        unlockTip.Init(MainManager.Data.FriendUnLockDataDict[name].unlockData);
    }

    private void OffUnLockTip()
    {
        unlockTip.gameObject.SetActive(false);
    }

    private void ListClear()
    {
        if (listSlots.Count <= 0)
            return;

        foreach(var  slot in listSlots)
            Destroy(slot.gameObject);

        listSlots.Clear();
    }

    private void HandlerClear()
    {
        UI_Game.ShopOpenHandler -= OnFriendShop;
        UI_FriendShopSlot.EnterSlotHandler -= OnUnLockTip;
        UI_FriendShopSlot.ExitSlotHandler -= OffUnLockTip;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
