using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendShop : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private Button exitButton;
    [SerializeField] private Transform content;
    //[SerializeField] private TODO : 잠금해제 조건 툴팁
    // TODO : 친구 해제 조건 만들기

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
        foreach(var info in MainManager.Data.FriendDataDict)
        {
            GameObject go = MainManager.Resource.Instantiate("FriendShop_Slot", content);
            UI_FriendShopSlot slot = go.GetComponent<UI_FriendShopSlot>();
            slot.Init(info.Key);
            listSlots.Add(slot);
        }

        exitButton.gameObject.AddEvent(OffFriendShop);
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
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
