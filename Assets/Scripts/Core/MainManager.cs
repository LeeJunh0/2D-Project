using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager instance;

    private static MainManager Instance { get { Init(); return instance; } }

    static private void Init()
    {
        GameObject go = GameObject.Find("MainManager");
        if(go == null)
        {
            go = new GameObject(name: "MainManager");
            go.AddComponent<MainManager>();
        }

        instance = go.GetComponent<MainManager>();
        DontDestroyOnLoad(go);
    }
}
