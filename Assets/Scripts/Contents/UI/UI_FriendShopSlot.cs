using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendShopSlot : UI_ScrollInButton
{
    public static event Action<string> BuyFriendHandler;

    [SerializeField] private Image slotIcon;
    [SerializeField] private Button buyButton;

    private string FriendName { get; set; }

    public void Init(string friendName)
    {
        SetEvent();

        FriendName = friendName;
        slotIcon.sprite = MainManager.Resource.LoadAtlas(MainManager.Data.FriendDataDict[name].friendIcon);
        buyButton.gameObject.AddEvent(BuyFriend);
    }

    private void BuyFriend(PointerEventData eventData)
    {
        BuyFriendHandler?.Invoke(FriendName);
    }
}
