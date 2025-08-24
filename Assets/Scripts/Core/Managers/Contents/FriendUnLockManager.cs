using System;
using UnityEngine;

public class FriendUnLockManager : MonoBehaviour
{
    private void Awake()
    {
        EventManager.OnFriendBuyHandler -= CheckUnLockData;
        EventManager.OnFriendSellHandler -= CheckUnLockData;
        EventManager.OnFriendBuyHandler += CheckUnLockData;
        EventManager.OnFriendSellHandler += CheckUnLockData;
    }

    private void CheckUnLockData(string name, UnlockActionType curAction)
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

    private void HandlerClear()
    {
        EventManager.OnFriendBuyHandler -= CheckUnLockData;
        EventManager.OnFriendSellHandler -= CheckUnLockData;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
