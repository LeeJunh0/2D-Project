using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Game : MonoBehaviour
{
    [SerializeField] private EUI_MenuType curMenu = EUI_MenuType.None;

    [Header("하단 BackGround")]
    [SerializeField] private GameObject menuBackGround;

    [Header("하단 세부UI")]
    [SerializeField] private List<GameObject> menus;

    [Header("하단 버튼들")]
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject optionButton;

    private void Awake()
    {
        upgradeButton.AddEvent(OnUpgrade);
        inventoryButton.AddEvent(OnInventory);
        optionButton.AddEvent(OnOption);
    }

    private void OnMenuBackGround()
    {
        menuBackGround.SetActive(true);
        MainManager.Cam.SetCameraDeptqh("TabCamera");
    }

    private void OffMenuBackGround()
    {
        menuBackGround.SetActive(false);
        MainManager.Cam.SetCameraDeptqh("MainCamera");
        curMenu = EUI_MenuType.None;
    }

    private void SetMenu(EUI_MenuType type)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if((int)type == i)
                menus[i].SetActive(true);
            else
                menus[i].SetActive(false);
        }
    }

    private void OnUpgrade(PointerEventData eventData)
    {
        if(curMenu == EUI_MenuType.Upgrade)
        {
            OffMenuBackGround();
            return;
        }

        OnMenuBackGround();
        curMenu = EUI_MenuType.Upgrade;
        SetMenu(curMenu);
    }

    private void OnInventory(PointerEventData eventData)
    {
        if(curMenu == EUI_MenuType.Inventory)
        {
            OffMenuBackGround();
            return;
        }

        OnMenuBackGround();
        curMenu = EUI_MenuType.Inventory;
        SetMenu(curMenu);
    }

    private void OnOption(PointerEventData eventData)
    {
        if (curMenu == EUI_MenuType.Option)
        {
            OffMenuBackGround();
            return;
        }

        OnMenuBackGround();
        curMenu = EUI_MenuType.Option;
        SetMenu(curMenu);
    }
}
