using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private EventBus eventBus;
    private FriendUnLockController unlockContoller;

    protected override void Awake()
    {
        base.Awake();

        eventBus = new EventBus();
        unlockContoller = new FriendUnLockController();
    }

    private void OnApplicationQuit()
    {
        PlayerDataManager.Instance.SaveData();
        OptionManager.Instance.SaveOptionData();
        EventBus.ResetEventBus();
    }
}
