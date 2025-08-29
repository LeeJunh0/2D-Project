using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<string, UnlockActionType> OnFriendBuyHandler;
    public static Action<int, UnlockActionType> OnFriendSellHandler;
    public static Action<string> OnUnLockSlotHandler;
    public static Action OnFriendCountUpdateHandler;
    public static Action<string> OnGachaUpdateHandler;

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

    public static void FriendCountUpdate()
    {
        OnFriendCountUpdateHandler?.Invoke();
    }

    public static void GachaUpdate(string name)
    {
        OnGachaUpdateHandler?.Invoke(name);
    }
}
