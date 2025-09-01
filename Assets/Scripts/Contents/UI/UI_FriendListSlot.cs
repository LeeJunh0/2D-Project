using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendListSlot : UI_ScrollInButton
{
    public static event Action<UI_FriendListSlot> SelectFrinedCheckHandler;
    public static event Action<UI_FriendListSlot> WalkOrRestCheckHandler;
    

    [SerializeField] private GameObject outLine;
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
        friendIcon.sprite = MainManager.Resource.LoadAtlas(info.friendIcon);
        
        SetEvent();
        gameObject.AddEvent(OnClick);
        equipButton.gameObject.AddEvent(OnWalkOrRestClick);

        UI_FriendStatus.OnIndexUpdateHandler -= IndexUpdate;
        UI_FriendStatus.OnIndexUpdateHandler += IndexUpdate;
    }
    
    private void OnClick(PointerEventData eventData)
    {
        SelectFrinedCheckHandler?.Invoke(this);
        AudioManager.Instance.EffectPlay("Click");
    }

    private void OnWalkOrRestClick(PointerEventData eventData)
    {
        WalkOrRestCheckHandler?.Invoke(this);
        AudioManager.Instance.EffectPlay("Click");
    }

    private void IndexUpdate(int removeIndex)
    {
        if (removeIndex >= Index)
            return;

        Index--;
    }

    public void OnOutLine() { outLine.SetActive(true); }
    public void OffOutLine() { outLine.SetActive(false); }

    private void HandlerClear()
    {
        UI_FriendStatus.OnIndexUpdateHandler -= IndexUpdate;
    }

    private void OnDestroy()
    {
        HandlerClear();
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
