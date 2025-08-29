using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopGacha : MonoBehaviour
{
    public static Action<string, Define.EFriend_Rarity> OnFriendGetHandler;

    [SerializeField] private GameObject ui;
    [SerializeField] private TextMeshProUGUI getText;
    [SerializeField] private Animator renderTexture;
    [SerializeField] private Animator effectAnim;
    [SerializeField] private Button getButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button retryButton;

    private FriendInfo friend;
    private Define.EFriend_Rarity rarity;

    private void Start()
    {
        getButton.gameObject.AddEvent(FriendGet);
        leaveButton.gameObject.AddEvent(FriendLeave);
        retryButton.gameObject.AddEvent(GachaRetry);

        EventManager.OnGachaUpdateHandler -= FriendUpdate;
        EventManager.OnGachaUpdateHandler += FriendUpdate;
    }

    private void FriendUpdate(string name)
    {
        ui.SetActive(true);      

        rarity = FriendGacha.RarityRandom();
        friend = MainManager.Data.FriendDataDict[name];
        renderTexture.runtimeAnimatorController = MainManager.Addressable.Load<RuntimeAnimatorController>($"Anim_{friend.objectName}");
        effectAnim.CrossFade("Effect", 0.1f);
        getText.text = string.Format($"새로운 친구! {RarityToString(friend.name, rarity)}이 합류를 원합니다!");
    }

    private string RarityToString(string name, Define.EFriend_Rarity rarity)
    {
        switch (rarity)
        {
            case Define.EFriend_Rarity.Normal:
                return $"<color=black>{name}</color>";
            case Define.EFriend_Rarity.Rare:
                return $"<color=blue>{name}</color>";
            case Define.EFriend_Rarity.Named:
                return $"<color=green>{name}</color>";
            case Define.EFriend_Rarity.Boss:
                return $"<color=red>{name}</color>";
            default:
                return "";
        }
    }

    private void FriendGet(PointerEventData eventData)
    {
        OnFriendGetHandler?.Invoke(friend.objectName, rarity);
        ui.SetActive(false);
    }

    private void FriendLeave(PointerEventData eventData)
    {
        ui.SetActive(false);
    }

    private void GachaRetry(PointerEventData eventData)
    {
        UI_FriendShopSlot.BuyFriend(friend.objectName);
    }

    private void HandlerClear()
    {
        getButton.gameObject.RemoveEvent(FriendGet);
        leaveButton.gameObject.RemoveEvent(FriendLeave);
        EventManager.OnGachaUpdateHandler -= FriendUpdate;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
