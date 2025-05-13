using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras;

    public void SetCameraDeptqh(string name)
    {
        if (cameras.Count == 0)
            return;

        foreach(Camera cam in cameras)
        {
            if (name == cam.name)
                cam.depth = 1;
            else
                cam.depth = 0;
        }
    }
}
