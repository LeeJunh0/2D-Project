using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Title : MonoBehaviour
{
    [SerializeField] private GameObject game;

    [Header("타이틀 메뉴버튼들")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;

    [Header("타이틀 각 메뉴UI")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject exitUI;

    private void Awake()
    {
        DataInit();
        startButton.AddEvent(GameStart);
        exitButton.AddEvent((evt) => { exitUI.SetActive(!exitUI.activeSelf); });
    }

    private void DataInit()
    {
        PlayerDataManager.Instance.LoadData();
    }

    private void GameStart(PointerEventData eventData)
    {
        MainManager.Addressable.LoadAsyncAll<Object>("Game", (key, cur, total) =>
        {
            if (cur == total)
            {
                gameObject.SetActive(false);
                Camera.main.transform.DOMove(new Vector3(0f, 3.1f, -10f), 3f).OnComplete(() =>
                {
                    PlayerDataManager.Instance.IsLoadCompleted = true;
                    game.SetActive(true);               
                });
            }
        });
    }
}
