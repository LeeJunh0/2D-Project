using TMPro;
using UnityEngine;

public class UI_UnLockToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void Init(UnLockData unlockData)
    {
        string targetName = MainManager.Data.FriendDataDict[MainManager.Data.NumberDataDict[unlockData.objectNum].name_desc].name;
        string actionName;
        switch (unlockData.actionType)
        {
            case UnlockActionType.Buy:
                actionName = "구해온다.";
                break;
            case UnlockActionType.Sell:
                actionName = "팔아넘긴다.";
                break;
            default:
                actionName = "";
                break;
        }

        descriptionText.text = string.Format($"{targetName}(을)를 {unlockData.curCount}/{unlockData.actionCount}회 {actionName}");
    }
}
