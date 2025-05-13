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
            Debug.LogError($"{name}������ �ε���� ���� �����Դϴ�.");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;

        return go;
    }
}
