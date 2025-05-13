using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrow : MonoBehaviour
{
    [SerializeField] private Camera tabCamera;

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
            CameraMove();
    }

    private void CameraMove()
    {
        float X = Mathf.Clamp(Camera.main.transform.position.x + dir * Time.deltaTime * speed, -19.15f, 21f);
        Camera.main.transform.position = new Vector3(X, Camera.main.transform.position.y, Camera.main.transform.position.z);
        tabCamera.transform.position = new Vector3(X, tabCamera.transform.position.y, tabCamera.transform.position.z);
    }
}
