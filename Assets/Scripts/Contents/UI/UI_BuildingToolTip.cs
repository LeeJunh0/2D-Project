using TMPro;
using UnityEngine;

public class UI_BuildingToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void Init(BuildInfo buildData)
    {
        gameObject.SetActive(true);
        nameText.text = buildData.name;
        descriptionText.text = buildData.description;
    }
}
