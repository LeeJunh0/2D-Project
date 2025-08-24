using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<string, UnlockActionType> OnFriendBuyHandler;
    public static Action<string, UnlockActionType> OnFriendSellHandler;
    public static Action<string> OnUnLockSlotHandler;
    

    public static void UnLockActionBuy(string name)
    {
        OnFriendBuyHandler?.Invoke(name, UnlockActionType.Buy);
    }

    public static void UnLockActionSell(string name)
    {
        OnFriendSellHandler?.Invoke(name, UnlockActionType.Sell);
    }

    public static void UnLockSlotUI(string name)
    {
        OnUnLockSlotHandler?.Invoke(name);
    }
}
