using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectionSlot : MonoBehaviour
{
    [SerializeField] private Image slot_Background;
    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Init(string name, Define.EFriend_Rarity rarity)
    {
        nameText.text = MainManager.Data.FriendDataDict[name].name;
        slotIcon.sprite = MainManager.Resource.LoadAtlas(MainManager.Data.FriendDataDict[name].friendIcon);

        switch (rarity)
        {
            case Define.EFriend_Rarity.Normal:
                slot_Background.color = Color.white;
                break;
            case Define.EFriend_Rarity.Rare:
                slot_Background.color = Color.blue;
                break;
            case Define.EFriend_Rarity.Named:
                slot_Background.color = Color.yellow;
                break;
            case Define.EFriend_Rarity.Boss:
                slot_Background.color = Color.red;
                break;
        }
    }
}
