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
        SetUnLock(MainManager.Data.FriendUnLockDataDict[friendName].unlockData.isCompleted);

        string spritePath = MainManager.Data.FriendDataDict[friendName].friendIcon;
        slotIcon.sprite = MainManager.Resource.LoadAtlas(spritePath);
        goldText.text = MainManager.Data.FriendDataDict[friendName].price.ToString();
    }

    public void SetUnLock(bool isComplete)
    {
        filter.SetActive(isComplete);
        if (isComplete)
            buyButton.gameObject.AddEvent(BuyFriend);
        else
        {
            gameObject.AddEvent(OnEnter, Define.EEvent_Type.Enter);
            gameObject.AddEvent(OnExit, Define.EEvent_Type.Exit);
        }
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
