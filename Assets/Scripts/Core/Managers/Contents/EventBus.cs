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

    // 잠금 해제 조건이 "구매" 이며 UI에 조건 현황 업데이트
    public static void UnLockActionBuy(string name)
    {
        OnFriendBuyHandler?.Invoke(name, UnlockActionType.Buy);
    }

    // 잠금 해제 조건이 "판매" 이며 UI에 조건 현황 업데이트
    public static void UnLockActionSell(int index)
    {
        OnFriendSellHandler?.Invoke(index, UnlockActionType.Sell);
    }

    // 상점 캐릭터슬롯 UI 잠금 해제 업데이트
    public static void UnLockSlotUI(string name)
    {
        OnUnLockSlotHandler?.Invoke(name);
    }

    // 가챠 UI 및 연출 업데이트
    public static void GachaUpdate(string name)
    {
        OnGachaUpdateHandler?.Invoke(name);
    }

    // 상점 캐릭터슬롯 ToolTip Enter
    public static void ShopSlotEnter(string name)
    {
        OnEnterSlotHandler?.Invoke(name);
    }

    // 캐릭터 카운팅 데이터 UI 업데이트
    public static void FriendCountUpdate()
    {
        OnFriendCountUpdateHandler?.Invoke();
    }

    // 상점 캐릭터슬롯 ToolTip Exit
    public static void ShopSlotExit()
    {
        OnExitSlotHandler?.Invoke();
    }

    // 해당 캐릭터를 살 수있는 재화가 있는지 확인 후 결과 반환
    public static bool BuyFriend(string name)
    {
        if (OnBuyFriendHandler == null)
            return false;

        bool result = OnBuyFriendHandler.Invoke(name);
        return result;
    }

    /// <summary>
    /// 게임 종료 null로 다 밀어버리면 된다.
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
