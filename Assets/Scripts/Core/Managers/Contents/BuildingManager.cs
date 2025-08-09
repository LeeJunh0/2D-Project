using System;
using System.Collections.Generic;
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

    private UI_BuildSlot curEnterSlot;

    public bool IsBuilding { get; set; }

    public BaseBuilding CurBuild
    {
        get { return curBuild; }
        set
        {
            curBuild = value;

            curBuild.Info = value.Info;
            SpriteRenderer spriteRenderer = preview.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = curBuild.GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }

    public UI_BuildSlot CurEnterSlot
    {
        get => curEnterSlot;
        set
        {
            curEnterSlot = value;
            if (curEnterSlot == null)
                return;

            tooltip.Init(curEnterSlot.InfoData);
        }
    }

    public void Init()
    {
        // 건물UI 슬롯 초기화
        BuildingSlotClear();
        foreach (var data in MainManager.Data.BuildDataDict)
        {
            GameObject go = MainManager.Resource.Instantiate("Building_Slot", content);
            UI_BuildSlot slot = go.GetComponent<UI_BuildSlot>();
            slot.Init(data.Value);
            buildSlots.Add(slot);
        }
    }

    public void ChoiceBuild(BaseBuilding build)
    {
        CurBuild = build;
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
}
