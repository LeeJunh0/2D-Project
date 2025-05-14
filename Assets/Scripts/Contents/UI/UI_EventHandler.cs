using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> onClick = null;
    public Action<PointerEventData> onEnter = null;
    public Action<PointerEventData> onExit = null;
    public Action<PointerEventData> onDown = null;
    public Action<PointerEventData> onUp = null;
    public Action<PointerEventData> onBeginDrag = null;
    public Action<PointerEventData> onDrag = null;
    public Action<PointerEventData> onEndDrag = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick == null)
            return;

        onClick.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(onEnter == null) 
            return;

        onEnter.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onExit == null)
            return;

        onExit.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(onDown == null)
            return;

        onDown.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp == null)
            return;

        onUp.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag == null)
            return;

        onBeginDrag.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag == null)
            return;

        onDrag.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(onEndDrag == null)
            return;

        onEndDrag.Invoke(eventData);
    }
}
