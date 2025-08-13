using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendListSlot : MonoBehaviour
{
    public static event Action<UI_FriendListSlot> SelectFrinedCheckHandler;
    public static event Action<UI_FriendListSlot> WalkOrRestCheckHandler;

    [SerializeField] private GameObject outLine;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image friendIcon;

    [SerializeField] private Image equipButton;
    [SerializeField] private TextMeshProUGUI equipText;
    [SerializeField] private GameObject equipBackGround;

    private bool isEquip;

    public int Index { get; set; }
    public bool IsEquip
    {
        get => isEquip;
        set
        {
            isEquip = value;

            if (isEquip == true)
            {
                equipBackGround.SetActive(true);
                equipText.text = string.Format("휴식!");
            }
            else
            {
                equipBackGround.SetActive(false);
                equipText.text = string.Format("노동!");
            }
        }
    }

    public void Init(FriendInfo info)
    {
        nameText.text = info.name;
        friendIcon.sprite = MainManager.Resource.LoadAtlas(info.friendIcon);

        gameObject.AddEvent(OnClick);
        equipButton.gameObject.AddEvent(OnWalkOrRestClick);
    }

    private void OnClick(PointerEventData eventData)
    {
        SelectFrinedCheckHandler?.Invoke(this);
    }

    private void OnWalkOrRestClick(PointerEventData eventData)
    {
        WalkOrRestCheckHandler?.Invoke(this);
    }

    public void OnOutLine() { outLine.SetActive(true); }
    public void OffOutLine() { outLine.SetActive(false); }
}
