public static class Define
{
    public enum EWorldObject_Type
    {
        None,
        Friend,
    }

    public enum EObject_State
    {
        Idle,
        Move,
        Doing,
    }

    public enum EFriend_Rarity
    {
        Normal = 0,
        Rare,
        Named,
        Boss
    }

    public enum EUI_MenuType
    {
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
