using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendShopSlot : UI_ScrollInButton
{
    public static event Action<string> BuyFriendHandler;
    public static event Action<string> EnterSlotHandler;
    public static event Action ExitSlotHandler;

    [SerializeField] private GameObject filter;
    [SerializeField] private Image slotIcon;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI goldText;

    private string FriendName { get; set; }

    public void Init(string friendName)
    {
        SetEvent();

        FriendName = friendName;
        if (MainManager.Data.FriendUnLockDataDict.ContainsKey(friendName) == false || MainManager.Data.FriendUnLockDataDict[friendName].unlockData.isCompleted == true)
        {
            filter.gameObject.SetActive(false);
            buyButton.gameObject.AddEvent(BuyFriend);
        }
        else
        {
            filter.gameObject.SetActive(true);
            gameObject.AddEvent(OnEnter, Define.EEvent_Type.Enter);
            gameObject.AddEvent(OnExit, Define.EEvent_Type.Exit);
        }

        string spritePath = MainManager.Data.FriendDataDict[friendName].friendIcon;
        slotIcon.sprite = MainManager.Resource.LoadAtlas(spritePath);
        goldText.text = MainManager.Data.FriendDataDict[friendName].price.ToString();
    }

    private void BuyFriend(PointerEventData eventData)
    {
        BuyFriendHandler?.Invoke(FriendName);
    }

    private void OnEnter(PointerEventData eventData)
    {
        EnterSlotHandler?.Invoke(FriendName);
    }

    private void OnExit(PointerEventData eventData)
    {
        ExitSlotHandler?.Invoke();
    }
}
