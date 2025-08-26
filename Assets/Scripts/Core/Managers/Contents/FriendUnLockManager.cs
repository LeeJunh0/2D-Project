using System;
using UnityEngine;

public class FriendUnLockManager : MonoBehaviour
{
    private void Awake()
    {
        EventManager.OnFriendBuyHandler -= BuyCheckUnLockData;
        EventManager.OnFriendSellHandler -= SellCheckUnLockData;
        EventManager.OnFriendBuyHandler += BuyCheckUnLockData;
        EventManager.OnFriendSellHandler += SellCheckUnLockData;
    }

    private void BuyCheckUnLockData(string name, UnlockActionType curAction)
    {
        foreach (var data in PlayerDataManager.Instance.UnLockData.unlockData.Values)
        {
            if (MainManager.Data.NumberDataDict[data.objectNum].name_desc == name && data.actionType == curAction)
            {
                data.CurCount++;
                EventManager.UnLockSlotUI(name);
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
                EventManager.UnLockSlotUI(name);
            }
        }
    }

    private void HandlerClear()
    {
        EventManager.OnFriendBuyHandler -= BuyCheckUnLockData;
        EventManager.OnFriendSellHandler -= SellCheckUnLockData;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
