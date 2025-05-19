using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            Camera.main.CameraMove(-1, 3f);

        if (Input.GetKey(KeyCode.D))
            Camera.main.CameraMove(1, 3f);
    }
}
