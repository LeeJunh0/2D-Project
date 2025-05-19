using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Arrow : MonoBehaviour
{
    [Header("카메라옵션")]
    [SerializeField] private int dir;
    [SerializeField] private int speed;
    [SerializeField] private bool isClick;

    private void Awake()
    {
        gameObject.AddEvent((evt) => { isClick = true; }, Define.EEvent_Type.Down);
        gameObject.AddEvent((evt) => { isClick = false; }, Define.EEvent_Type.Up);
    }

    private void Update()
    {
        if (isClick == true)
            Camera.main.CameraMove(dir, speed);               
    }
}
