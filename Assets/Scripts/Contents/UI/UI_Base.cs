using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public abstract class UI_Base : MonoBehaviour
{


    public abstract void Show();
    public abstract void Close(); 
    
    public static void AddEvent(GameObject go, Action<PointerEventData> action, Event_Type type = Event_Type.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Event_Type.Click:
                evt.onClickHandler -= action;
                evt.onClickHandler += action;
                break;
            case Event_Type.Down:
                evt.onDownHandler -= action;
                evt.onDownHandler += action;
                break;
            case Event_Type.Up:
                evt.onUpHandler -= action;
                evt.onUpHandler += action;
                break;
        }
    }
}
