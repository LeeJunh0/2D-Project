using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuildSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI priceText;

    public BuildInfo InfoData { get; set; }
    public void Init(BuildInfo buildData)
    {
        InfoData = buildData;

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
        BuildingManager.Instance.ChoiceBuild(build);
    }

    private void OnEnter(PointerEventData eventData)
    {
        BuildingManager.Instance.CurEnterSlot = this;
    }

    private void OnExit(PointerEventData eventData)
    {
        BuildingManager.Instance.OffBuildingToolTip();   
    }
}
