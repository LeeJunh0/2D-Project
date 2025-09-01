using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendShopSlot : UI_ScrollInButton
{
    public static event Action<UI_FriendShopSlot> UnLockSlotHandler;
    public static event Func<string, bool> BuyFriendHandler;
    public static event Action<string> EnterSlotHandler;
    public static event Action ExitSlotHandler;

    [SerializeField] private GameObject filter;
    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI goldText;

    public string FriendName { get; set; }

    public void Init(string friendName)
    {
        SetEvent();

        FriendName = friendName;

        string spritePath = MainManager.Data.FriendDataDict[friendName].friendIcon;
        slotIcon.sprite = MainManager.Resource.LoadAtlas(spritePath);
        goldText.text = MainManager.Data.FriendDataDict[friendName].price.ToString("N0");
    }

    public void SetUnLock(bool isUnLock)
    {
        filter.SetActive(!isUnLock);
        if (isUnLock == true)
        {
            gameObject.RemoveEvent(OnEnter, Define.EEvent_Type.Enter);
            gameObject.RemoveEvent(OnExit, Define.EEvent_Type.Exit);
            gameObject.AddEvent(OnBuyFriend);
        }
        else
        {
            gameObject.AddEvent(OnEnter, Define.EEvent_Type.Enter);
            gameObject.AddEvent(OnExit, Define.EEvent_Type.Exit);
            gameObject.RemoveEvent(OnBuyFriend);
        }
    }

    private void OnBuyFriend(PointerEventData eventData)
    {
        AudioManager.Instance.EffectPlay("Click");
        BuyFriend(FriendName);
    }

    public static void BuyFriend(string name)
    {
        if (BuyFriendHandler?.Invoke(name) == false)
            return;

        EventManager.GachaUpdate(name);
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
