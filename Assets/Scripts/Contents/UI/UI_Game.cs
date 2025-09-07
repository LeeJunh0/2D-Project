using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Game : MonoBehaviour
{
    public static event Action<bool> StatusOpenHandler;
    public static event Action<bool> BuildingOpenHandler;
    public static event Action<bool> ShopOpenHandler;
    public static event Action<bool> CollectionOpenHandler;

    [SerializeField] private EUI_MenuType curMenu = EUI_MenuType.None;

    [Header("좌측 BackGround")]
    [SerializeField] private RectTransform menuBackGround;
    [SerializeField] private bool isOpen = false;

    [Header("좌측 세부UI")]
    [SerializeField] private List<GameObject> menus;

    [Header("하단 버튼들")]
    [SerializeField] private GameObject statusButton;
    [SerializeField] private GameObject buildingButton;
    [SerializeField] private GameObject shopButton;
    [SerializeField] private GameObject collectionButton;
    [SerializeField] private GameObject optionButton;

    [Header("옵션창 UI들")]
    [SerializeField] private Toggle pinToggle;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button quitButton;

    public EUI_MenuType MenuType
    {
        get { return curMenu; }
        set
        {
            curMenu = value;
            for (int i = 0; i < menus.Count; i++)
            {
                if (i == (int)curMenu)
                {
                    menus[i].SetActive(true);
                    AudioManager.Instance.EffectPlay("Open");
                }
                else
                    menus[i].SetActive(false);
            }
        }
    }

    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            isOpen = value;

            if (value == true)
                menuBackGround.DOAnchorPosX(203f, 0.5f);
            else
            {
                menuBackGround.DOAnchorPosX(-385f, 0.5f);
                AudioManager.Instance.EffectPlay("Close");
            }
        }
    }

    private void Awake()
    {
        statusButton.AddEvent(FriendStatusButton);
        buildingButton.AddEvent(BuildingShopButton);
        collectionButton.AddEvent(FriendCollectionButton);
        shopButton.AddEvent(FriendShopButton);
        optionButton.AddEvent((evt) => { SetMenu(EUI_MenuType.Option); });
        OptionInit();

        BuildingManager.BuildingStartHandler -= BuildingTabClose;
        BuildingManager.BuildingStartHandler += BuildingTabClose;
    }

    private void OptionInit()
    {
        pinToggle.isOn = OptionManager.Instance.IsWindowPin;
        muteToggle.isOn = OptionManager.Instance.IsMute;

        pinToggle.onValueChanged.RemoveListener(OptionManager.Instance.OptionWindowPinToggle);
        pinToggle.onValueChanged.AddListener(OptionManager.Instance.OptionWindowPinToggle);
        muteToggle.onValueChanged.RemoveListener(OptionManager.Instance.OptionMuteToggle);
        muteToggle.onValueChanged.AddListener(OptionManager.Instance.OptionMuteToggle);

        bgmSlider.value = OptionManager.Instance.CurBGM / OptionManager.MaxBGM;
        effectSlider.value = OptionManager.Instance.CurEffect / OptionManager.MaxEffect;

        bgmSlider.onValueChanged.RemoveListener(OptionManager.Instance.OptionBgmValueSet);
        bgmSlider.onValueChanged.AddListener(OptionManager.Instance.OptionBgmValueSet);
        effectSlider.onValueChanged.RemoveListener(OptionManager.Instance.OptionEffectValueSet);
        effectSlider.onValueChanged.AddListener(OptionManager.Instance.OptionEffectValueSet);

        saveButton.onClick.RemoveListener(PlayerDataManager.Instance.SaveData);
        saveButton.onClick.AddListener(PlayerDataManager.Instance.SaveData);
        quitButton.onClick.RemoveListener(Application.Quit);
        quitButton.onClick.AddListener(Application.Quit);
    }

    private void SetMenu(EUI_MenuType type)
    {
        if (type == curMenu)
        {
            IsOpen = false;
            curMenu = EUI_MenuType.None;
            return;
        }

        MenuType = type;
        IsOpen = true;
    }

    private void FriendStatusButton(PointerEventData eventData)
    {
        SetMenu(EUI_MenuType.Status);
        StatusOpenHandler?.Invoke(IsOpen);
    }

    private void BuildingShopButton(PointerEventData eventData)
    {
        SetMenu(EUI_MenuType.Building);
        BuildingOpenHandler?.Invoke(IsOpen);
    }

    private void FriendShopButton(PointerEventData eventData)
    {
        SetMenu(EUI_MenuType.Shop);
        ShopOpenHandler?.Invoke(IsOpen);
    }

    private void FriendCollectionButton(PointerEventData eventData)
    {
        SetMenu(EUI_MenuType.Collection);
        CollectionOpenHandler?.Invoke(IsOpen);
    }

    private void BuildingTabClose()
    {
        SetMenu(EUI_MenuType.Building);
    }
}
