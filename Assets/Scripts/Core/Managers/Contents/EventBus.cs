using System;
using UnityEngine;

public class EventBus
{
    public static event Action<string, UnlockActionType> OnFriendBuyHandler;
    public static event Action<int, UnlockActionType> OnFriendSellHandler;
    public static event Action<string> OnUnLockSlotHandler;
    public static event Action<string> OnGachaUpdateHandler;
    public static event Action<string> OnEnterSlotHandler;
    public static event Action OnFriendCountUpdateHandler;
    public static event Action OnExitSlotHandler;

    public static event Func<string, bool> OnBuyFriendHandler;

    public static void UnLockActionBuy(string name)
    {
        OnFriendBuyHandler?.Invoke(name, UnlockActionType.Buy);
    }

    public static void UnLockActionSell(int index)
    {
        OnFriendSellHandler?.Invoke(index, UnlockActionType.Sell);
    }

    public static void UnLockSlotUI(string name)
    {
        OnUnLockSlotHandler?.Invoke(name);
    }

    public static void GachaUpdate(string name)
    {
        OnGachaUpdateHandler?.Invoke(name);
    }

    public static void ShopSlotEnter(string name)
    {
        OnEnterSlotHandler?.Invoke(name);
    }

    public static void FriendCountUpdate()
    {
        OnFriendCountUpdateHandler?.Invoke();
    }

    public static void ShopSlotExit()
    {
        OnExitSlotHandler?.Invoke();
    }

    public static bool BuyFriend(string name)
    {
        if (OnBuyFriendHandler == null)
            return false;

        bool result = OnBuyFriendHandler.Invoke(name);
        return result;
    }

    /// <summary>
    /// 게임 종료
    /// null로 다 밀어버리면 된다.
    /// </summary>
    public static void ResetEventBus()
    {
        OnFriendBuyHandler = null;
        OnFriendSellHandler = null;
        OnUnLockSlotHandler = null;
        OnFriendCountUpdateHandler = null;
        OnGachaUpdateHandler = null;
        OnEnterSlotHandler = null;
        OnExitSlotHandler = null;
        OnBuyFriendHandler = null;
    }
}
