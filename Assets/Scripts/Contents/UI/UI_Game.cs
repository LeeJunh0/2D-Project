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

    }

    private void OnEnable()
    {
        MainManager.PlayerData.TextUpdate();
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
}
