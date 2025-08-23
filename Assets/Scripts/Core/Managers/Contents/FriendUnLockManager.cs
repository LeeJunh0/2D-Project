using System;
using UnityEditor.AddressableAssets.Build;
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
        foreach (var data in MainManager.Data.FriendUnLockDataDict.Values)
        {
            if (MainManager.Data.NumberDataDict[data.unlockData.objectNum].name_desc == name && data.unlockData.actionType == curAction)
                data.unlockData.CurCount++;
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
