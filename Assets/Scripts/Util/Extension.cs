using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

static public class Extension
{
    static public T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        if(go.GetComponent<T>() == null)
            return go.AddComponent<T>();

        return go.GetComponent<T>();
    }

    static public T FindChild<T>(this GameObject go) where T : Component
    {
        T child = go.GetComponentInChildren<T>();
        if (child == null)
            return null;

        return child;
    }

    public static void AddEvent(this GameObject go, Action<PointerEventData> action, Define.EEvent_Type type = Define.EEvent_Type.Click)
    {
        UIEventHandler evt = GetOrAddComponent<UIEventHandler>(go);

        switch (type)
        {
            case Define.EEvent_Type.Click:
                evt.onClick += action;
                break;
            case Define.EEvent_Type.Enter:
                evt.onEnter += action;
                break;
            case Define.EEvent_Type.Exit:
                evt.onExit += action;
                break;
            case Define.EEvent_Type.Down:
                evt.onDown += action;
                break;
            case Define.EEvent_Type.Up:
                evt.onUp += action;
                break;
            case Define.EEvent_Type.BeginDrag:
                evt.onBeginDrag += action;
                break;
            case Define.EEvent_Type.Drag:
                evt.onDrag += action;
                break;
            case Define.EEvent_Type.EndDrag:
                evt.onEndDrag += action;
                break;
        }
    }
}
