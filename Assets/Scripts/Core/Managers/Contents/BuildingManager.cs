using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private Preview preview;
    [SerializeField] private BaseBuilding curBuild;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cover;
    [SerializeField] private List<UI_BuildSlot> buildSlots;
    [SerializeField] private UI_BuildingToolTip tooltip;

    public Transform BuildParent;

    public bool IsBuilding { get; set; }
    public void Init()
    {
        // 건물UI 슬롯 초기화
        BuildingSlotClear();
        foreach (var data in MainManager.Data.BuildDataDict)
        {
            if (data.Value.objectName == "House")
                continue;

            GameObject go = MainManager.Resource.Instantiate("Building_Slot", content);
            UI_BuildSlot slot = go.GetComponent<UI_BuildSlot>();
            slot.Init(data.Value);
            buildSlots.Add(slot);
        }

        UI_BuildSlot.OnToolTipHandler -= tooltip.Init;
        UI_BuildSlot.OnToolTipHandler += tooltip.Init;
        UI_BuildSlot.OffToolTipHandler -= OffBuildingToolTip;
        UI_BuildSlot.OffToolTipHandler += OffBuildingToolTip;
        UI_BuildSlot.OnBuildModeHandler -= ChoiceBuild;
        UI_BuildSlot.OnBuildModeHandler += ChoiceBuild;
    }

    public void ChoiceBuild(BaseBuilding build)
    {
        curBuild = build;
        SpriteRenderer spriteRenderer = preview.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = curBuild.GetComponentInChildren<SpriteRenderer>().sprite;
        PreviewSetting();
    }

    private void PreviewSetting()
    {
        OnPreview();

        BoxCollider2D previewColl = preview.GetComponent<BoxCollider2D>();
        BoxCollider2D curColl = curBuild.GetComponentInChildren<BoxCollider2D>();
        previewColl.size = curColl.size;
        previewColl.offset = curColl.offset;
    }

    public void OnPreview()
    {
        preview.gameObject.SetActive(true);
        cover.SetActive(true);
    }

    public void OffPreview()
    {
        preview.gameObject.SetActive(false);
        cover.SetActive(false);
    }

    public void BuildMaterialzation()
    {
        GameObject go = MainManager.Resource.Instantiate(curBuild.Info.objectName);
        go.transform.position = preview.transform.position;
        go.transform.parent = BuildParent;

        BaseBuilding baseBuilding = go.GetComponent<BaseBuilding>();
        baseBuilding.Info = curBuild.Info;
        PlayerDataManager.Instance.AddBuild(baseBuilding);
        OffPreview();
    }

    public void OffBuildingToolTip()
    {
        tooltip.gameObject.SetActive(false);
    }

    private void BuildingSlotClear()
    {
        if (buildSlots.Count <= 0)
            return;

        foreach (var slot in buildSlots)
            Destroy(slot.gameObject);

        buildSlots.Clear();
    }

    private void HandlerClear()
    {
        UI_BuildSlot.OnBuildModeHandler -= ChoiceBuild;
        UI_BuildSlot.OnToolTipHandler -= tooltip.Init;
        UI_BuildSlot.OffToolTipHandler -= OffBuildingToolTip;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
