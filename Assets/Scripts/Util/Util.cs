using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Util 
{
    static public T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        if(go.GetComponent<T>() == null)
            return go.AddComponent<T>();

        return go.GetComponent<T>();
    }

    static public T FindChild<T>(GameObject go) where T : Component
    {
        T child = go.GetComponentInChildren<T>();
        if (child == null)
            return null;

        return child;

    }
}
