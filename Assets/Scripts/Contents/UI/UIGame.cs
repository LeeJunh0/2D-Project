using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Util;

public class UIGame : MonoBehaviour
{
    [Header("�� ���� �޴���ư��")]
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject optionMenu;

    [SerializeField] private GameObject curMenu = null;

    private void OnUpgradeMenu()
    {
        if(curMenu != null && upgradeMenu.activeSelf == false)
        {
            curMenu.SetActive(false);
            upgradeMenu.SetActive(true);
            curMenu = upgradeMenu;
        } 
    }
}
