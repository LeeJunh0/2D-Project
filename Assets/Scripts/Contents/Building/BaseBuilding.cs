using UnityEngine;
using static Define;

public class BaseBuilding : MonoBehaviour
{
    protected BoxCollider2D boxColl;
    protected bool isUse = false;

    public BuildInfo Info { get; set; }

    private void Awake()
    {
        Init();
        gameObject.AddEvent((evt) => Interacting(), EEvent_Type.LeftClick);
    }

    protected virtual void Init() { boxColl = GetComponent<BoxCollider2D>(); }

    protected virtual void Interacting() { BuildingManager.Instance.ChoiceBuild(this);  }
}
