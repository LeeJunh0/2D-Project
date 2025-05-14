using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Title : MonoBehaviour
{
    [Header("Ÿ��Ʋ �޴���ư��")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;

    [Header("Ÿ��Ʋ �� �޴�UI")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject exitUI;

    private void Awake()
    {
        startButton.AddEvent(GameStart);
        exitButton.AddEvent((evt) => { exitUI.SetActive(!exitUI.activeSelf); });
    }

    private void GameStart(PointerEventData eventData)
    {
        MainManager.Loading.SceneLoading("Game");
    }
}
