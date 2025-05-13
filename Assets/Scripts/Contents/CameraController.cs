using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }


}
