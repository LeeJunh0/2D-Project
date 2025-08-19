using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendShop : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Transform content;
    //[SerializeField] private TODO : 잠금해제 조건 툴팁

    private List<UI_FriendShopSlot> listSlots;

    public void Init()
    {
        ListClear();
        foreach(var info in MainManager.Data.FriendDataDict)
        {
            GameObject go = MainManager.Resource.Instantiate("UI_FriendShopSlot", content);
            UI_FriendShopSlot slot = go.GetComponent<UI_FriendShopSlot>();
            slot.Init(info.Key);
            listSlots.Add(slot);
        }

        exitButton.gameObject.AddEvent(OffFriendShop);
    }

    private void OffFriendShop(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }

    private void ListClear()
    {
        if (listSlots.Count <= 0)
            return;

        foreach(var  slot in listSlots)
            Destroy(slot.gameObject);

        listSlots.Clear();
    }
}
