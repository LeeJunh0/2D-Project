using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuildSlot : UI_ScrollInButton
{
    public static Action<BaseBuilding> OnBuildModeHandler;
    public static Action<BuildInfo> OnToolTipHandler;
    public static Action OffToolTipHandler;

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI priceText;

    public BuildInfo InfoData { get; set; }
    public void Init(BuildInfo buildData)
    {
        InfoData = buildData;
        SetEvent();
        icon.sprite = MainManager.Resource.LoadAtlas(buildData.buildIcon);
        priceText.text = buildData.price.ToString();

        gameObject.AddEvent(OnClick);
        gameObject.AddEvent(OnEnter, Define.EEvent_Type.Enter);
        gameObject.AddEvent(OnExit, Define.EEvent_Type.Exit);
    }

    private void OnClick(PointerEventData eventData)
    {
        GameObject go = MainManager.Addressable.Load<GameObject>(InfoData.objectName);
        BaseBuilding build = go.GetComponent<BaseBuilding>();
        build.Info = InfoData;

        OnBuildModeHandler?.Invoke(build);
    }

    private void OnEnter(PointerEventData eventData)
    {
        OnToolTipHandler?.Invoke(InfoData);
    }

    private void OnExit(PointerEventData eventData)
    {
        OffToolTipHandler?.Invoke();
    }
}
