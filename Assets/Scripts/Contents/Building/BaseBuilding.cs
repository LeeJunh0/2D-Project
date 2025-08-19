using UnityEngine;
using static Define;

public class BaseBuilding : MonoBehaviour
{
    protected bool isUse = false;

    public BuildInfo Info { get; set; }

    private void Awake()
    {
        Init();
        gameObject.AddEvent((evt) => Interacting(), EEvent_Type.LeftClick);
    }

    protected virtual void Init() { }

    protected virtual void Interacting() {  }
}
