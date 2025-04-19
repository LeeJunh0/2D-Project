using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Util;

public abstract class UIBase : MonoBehaviour
{
    public void Awake()
    {
        Init();
    }

    public abstract void Init();
}
