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

    [Header("�ϴ� BackGround")]
    [SerializeField] private GameObject menuBackGround;

    [Header("�ϴ� ����UI")]
    [SerializeField] private List<GameObject> menus;

    [Header("�ϴ� ��ư��")]
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
