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
}
