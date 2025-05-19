using System.Collections;
using System.Collections.Generic;

public static class Define
{
    public enum EWorldObject_Type
    {
        None,
        Monster,
    }

    public enum EObject_State
    {
        Idle,
        Move,
        Doing,
    }

    public enum EUI_MenuType
    {
        Upgrade = 0,
        Inventory,
        Option,
        None
    }

    public enum EEvent_Type
    {
        Click,
        Enter,
        Exit,
        Down,
        Up,
        BeginDrag,
        Drag,
        EndDrag
    }

    public enum EWindowPos
    {
        Top,
        Bottom
    }
}
