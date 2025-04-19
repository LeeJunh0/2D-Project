using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Util;

public class Grid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AddEvent(gameObject, OnHighLight, Define.Event_Type.Enter);
        AddEvent(gameObject, OffHighLight, Define.Event_Type.Exit);
    }

    private void OnHighLight(PointerEventData eventData)
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void OffHighLight(PointerEventData eventData)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }
}
