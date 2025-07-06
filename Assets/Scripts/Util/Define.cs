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

    public enum EMonster_Rarity
    {
        Normal,
        Rare,
        Unique,
        Legend
    }

    public enum EUI_MenuType
    {
        Status = 0,
        Building,
        Option,
        None
    }

    public enum EEvent_Type
    {
        LeftClick,
        RightClick,
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
