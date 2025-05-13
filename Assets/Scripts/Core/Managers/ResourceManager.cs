using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager
{
    public GameObject Instantiate(string name, Transform parent = null)
    {
        GameObject prefab = MainManager.Addressable.Load<GameObject>(name);
        if(prefab == null)
        {
            Debug.LogError($"{name}에셋은 로드되지 않은 에셋입니다.");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;

        return go;
    }
}
