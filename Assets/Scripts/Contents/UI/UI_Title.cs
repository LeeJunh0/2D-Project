using DG.Tweening;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Title : MonoBehaviour
{
    public static Action OnWindowPinHandler;
    public static Action OnMuteHandler;

    [SerializeField] private GameObject game;

    [Header("타이틀 메뉴버튼들")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button optionButton;

    [Header("타이틀 각 메뉴UI")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject exitUI;

    [SerializeField] private Button optionExitButton;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Toggle windowPinToggle;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;

    [SerializeField] private Button quitYesButton;
    [SerializeField] private Button quitNoButton;

    private void Awake()
    {
        startButton.gameObject.AddEvent(GameStart);
        optionButton.gameObject.AddEvent(OptionUIOpen);
        exitButton.gameObject.AddEvent(ExitUIOpen);
    }

    private void GameStart(PointerEventData eventData)
    {
        AudioManager.Instance.EffectPlay("Click");
        MainManager.Addressable.LoadAsyncAll<UnityEngine.Object>("Game", (key, cur, total) =>
        {
            if (cur == total)
            {
                PlayerDataManager.Instance.LoadData();
                gameObject.SetActive(false);
                Camera.main.transform.DOMove(new Vector3(0f, 3.1f, -10f), 3f).OnComplete(() =>
                {
                    PlayerDataManager.Instance.IsLoadCompleted = true;
                    game.SetActive(true);
                });
            }
        });
    }

    private void OptionUIOpen(PointerEventData eventData)
    {
        optionUI.SetActive(true);
        AudioManager.Instance.EffectPlay("Click");
        OptionUIInit();
    }

    private void ExitUIOpen(PointerEventData eventData)
    {
        exitUI.SetActive(true);
        AudioManager.Instance.EffectPlay("Click");
    }

    private void OptionUIInit()
    {
        windowPinToggle.isOn = OptionManager.Instance.IsWindowPin;
        muteToggle.isOn = OptionManager.Instance.IsMute;

        windowPinToggle.onValueChanged.RemoveListener(OptionWindowPinToggle);
        windowPinToggle.onValueChanged.AddListener(OptionWindowPinToggle);
        muteToggle.onValueChanged.RemoveListener(OptionMuteToggle);
        muteToggle.onValueChanged.AddListener(OptionMuteToggle);

        bgmSlider.value = OptionManager.Instance.CurBGM / OptionManager.MaxBGM;
        effectSlider.value = OptionManager.Instance.CurEffect / OptionManager.MaxEffect;

        bgmSlider.onValueChanged.RemoveListener(OptionBgmValueSet);
        bgmSlider.onValueChanged.AddListener(OptionBgmValueSet);
        effectSlider.onValueChanged.RemoveListener(OptionEffectValueSet);
        effectSlider.onValueChanged.AddListener(OptionEffectValueSet);

        optionExitButton.gameObject.AddEvent(OptionUIClose);
    }

    private void OptionWindowPinToggle(bool isWindowPin)
    {
        OptionManager.Instance.IsWindowPin = isWindowPin;
        AudioManager.Instance.EffectPlay("Click");
    }

    private void OptionMuteToggle(bool isMute)
    {
        OptionManager.Instance.IsMute = isMute;
        AudioManager.Instance.EffectPlay("Click");
    }

    private void OptionBgmValueSet(float value)
    {
        OptionManager.Instance.CurBGM = value;
    }

    private void OptionEffectValueSet(float value)
    {
        OptionManager.Instance.CurEffect = value;
    }

    private void OptionUIClose(PointerEventData eventData)
    {
        optionUI.SetActive(false);
        AudioManager.Instance.EffectPlay("Close");
    }

    private void QuitGame(PointerEventData eventData)
    {
        Application.Quit();
    }

    private void ExitCloseUI(PointerEventData eventData)
    {
        AudioManager.Instance.EffectPlay("Close");
        exitUI.SetActive(false);
    }
}
