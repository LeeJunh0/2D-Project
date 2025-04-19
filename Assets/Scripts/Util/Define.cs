using System.Collections;
using System.Collections.Generic;

public static class Define
{
    public enum WorldObject_Type
    {
        None,
        Monster,
    }

    public enum Object_State
    {
        Idle,
        Move,
        Doing,
    }

    public enum Event_Type
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

}
