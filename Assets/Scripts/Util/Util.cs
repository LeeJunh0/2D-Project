using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

static public class Util 
{
    static public T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        if(go.GetComponent<T>() == null)
            return go.AddComponent<T>();

        return go.GetComponent<T>();
    }

    static public T FindChild<T>(GameObject go) where T : Component
    {
        T child = go.GetComponentInChildren<T>();
        if (child == null)
            return null;

        return child;
    }

    public static void AddEvent(GameObject go, Action<PointerEventData> action, Define.Event_Type type = Define.Event_Type.Click)
    {
        UIEventHandler evt = GetOrAddComponent<UIEventHandler>(go);

        switch (type)
        {
            case Define.Event_Type.Click:
                evt.onClick += action;
                break;
            case Define.Event_Type.Enter:
                evt.onEnter += action;
                break;
            case Define.Event_Type.Exit:
                evt.onExit += action;
                break;
            case Define.Event_Type.Down:
                evt.onDown += action;
                break;
            case Define.Event_Type.Up:
                evt.onUp += action;
                break;
            case Define.Event_Type.BeginDrag:
                evt.onBeginDrag += action;
                break;
            case Define.Event_Type.Drag:
                evt.onDrag += action;
                break;
            case Define.Event_Type.EndDrag:
                evt.onEndDrag += action;
                break;
        }
    }
}
