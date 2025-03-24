using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager
{
    public GameObject LoadPrefab(string path) 
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + path);
        GameObject go = Object.Instantiate(prefab);
        if(go == null)
        {
            Debug.LogError($"{path}라는 프리팹은 존재 하지않습니다.");
            return null;
        }

        return go;
    }

    public GameObject LoadPrefab<T>() where T : Component
    {
        GameObject go = LoadPrefab(typeof(T).Name);
        return go;
    }
}
