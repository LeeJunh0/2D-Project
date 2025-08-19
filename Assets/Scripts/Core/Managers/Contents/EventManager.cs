using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<string, UnlockActionType> OnFriendBuyHandler;
    public static Action<string, UnlockActionType> OnFriendSellHandler;

    public static void UnLockActionBuy(string name)
    {
        OnFriendBuyHandler?.Invoke(name, UnlockActionType.Buy);
    }

    public static void UnLockActionSell(string name)
    {
        OnFriendSellHandler?.Invoke(name, UnlockActionType.Sell);
    }
}
