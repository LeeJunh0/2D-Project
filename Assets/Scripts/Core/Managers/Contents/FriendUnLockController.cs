using System;
using UnityEngine;

public class FriendUnLockController
{
    public FriendUnLockController()
    {
        EventBus.OnFriendBuyHandler -= BuyCheckUnLockData;
        EventBus.OnFriendSellHandler -= SellCheckUnLockData;
        EventBus.OnFriendBuyHandler += BuyCheckUnLockData;
        EventBus.OnFriendSellHandler += SellCheckUnLockData;
    }

    private void BuyCheckUnLockData(string name, UnlockActionType curAction)
    {
        foreach (var data in PlayerDataManager.Instance.UnLockData.unlockData.Values)
        {
            if (MainManager.Data.NumberDataDict[data.objectNum].name_desc == name && data.actionType == curAction)
            {
                data.CurCount++;
                EventBus.UnLockSlotUI(name);
            }
        }
    }

    private void SellCheckUnLockData(int index, UnlockActionType curAction)
    {
        Friend friend = PlayerDataManager.Instance.FriendList[index];

        foreach (var data in PlayerDataManager.Instance.UnLockData.unlockData.Values)
        {
            if (MainManager.Data.NumberDataDict[data.objectNum].name_desc == friend.Stat.info.name && data.actionType == curAction)
            {
                data.CurCount++;
                EventBus.UnLockSlotUI(friend.Stat.info.name);
            }
        }
    }
}
