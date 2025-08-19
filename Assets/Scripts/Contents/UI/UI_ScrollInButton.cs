using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_ScrollInButton : MonoBehaviour
{
    private ScrollRect parentScroll;

    protected virtual void SetEvent()
    {
        parentScroll = transform.parent.parent.parent.GetComponent<ScrollRect>();

        gameObject.AddEvent(OnBeginDrag,Define.EEvent_Type.BeginDrag);
        gameObject.AddEvent(OnDrag,Define.EEvent_Type.Drag);
        gameObject.AddEvent(OnEndDrag,Define.EEvent_Type.EndDrag);
    }

    protected virtual void OnBeginDrag(PointerEventData eventData)
    {
        parentScroll.OnBeginDrag(eventData);
    }

    protected virtual void OnDrag(PointerEventData eventData)
    {
        parentScroll.OnDrag(eventData);
    }

    protected virtual void OnEndDrag(PointerEventData eventData)
    {
        parentScroll.OnEndDrag(eventData);
    }
}
