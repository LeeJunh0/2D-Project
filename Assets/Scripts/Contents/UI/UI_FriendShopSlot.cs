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
        string spritePath = MainManager.Data.FriendDataDict[friendName].friendIcon;
        slotIcon.sprite = MainManager.Resource.LoadAtlas(spritePath);
        buyButton.gameObject.AddEvent(BuyFriend);
    }

    private void BuyFriend(PointerEventData eventData)
    {
        BuyFriendHandler?.Invoke(FriendName);
    }
}
