using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class UIArrow : UIBase
{
    [Header("카메라옵션")]
    [SerializeField] private int dir;
    [SerializeField] private int speed;
    [SerializeField] private bool isClick;

    public override void Init()
    {
        AddEvent(gameObject, (evt) => { isClick = true; }, Define.Event_Type.Down);
        AddEvent(gameObject, (evt) => { isClick = false; }, Define.Event_Type.Up);
    }

    private void Update()
    {
        if (isClick == true)
            CameraMove();
    }

    private void CameraMove()
    {
        float X = Mathf.Clamp(Camera.main.transform.position.x + dir * Time.deltaTime * speed, -19.15f, 21f);
        Camera.main.transform.position = new Vector3(X, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
}
