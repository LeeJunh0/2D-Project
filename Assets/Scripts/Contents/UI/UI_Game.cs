using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public EUI_MenuType MenuType
    {
        get { return curMenu; }
        set
        {
            curMenu = value;

            for (int i = 0; i < menus.Count; i++)
            {
                if (i == (int)curMenu)
                    menus[i].SetActive(true);
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
                menuBackGround.DOMoveX(203f, 0.5f);
            else
                menuBackGround.DOMoveX(-385f, 0.5f);
        }
    }

    private void Awake()
    {
        statusButton.AddEvent(FriendStatusButton);
        buildingButton.AddEvent(BuildingShopButton);
        collectionButton.AddEvent(FriendCollectionButton);
        shopButton.AddEvent(FriendShopButton);
        optionButton.AddEvent((evt) => { SetMenu(EUI_MenuType.Option); });
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

}
