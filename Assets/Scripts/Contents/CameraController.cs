using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            Camera.main.CameraMove(-1, 3f);

        if (Input.GetKey(KeyCode.D))
            Camera.main.CameraMove(1, 3f);
    }
}
