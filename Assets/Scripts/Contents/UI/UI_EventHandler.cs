using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<PointerEventData> onClickHandler = null;
    public Action<PointerEventData> onDownHandler = null;
    public Action<PointerEventData> onUpHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClickHandler == null)
            return;

        onClickHandler.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(onDownHandler == null) 
            return;

        onDownHandler.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(onUpHandler == null)
            return; 
        
        onUpHandler.Invoke(eventData);
    }
}
