using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager instance = null;

    private static MainManager Instance { get { Init(); return instance; } }

    private DataManager dataManager = new DataManager();
    private ResourceManager resourceManager = new ResourceManager();
    private UIManager uIManager = new UIManager();
    public static DataManager Data { get { return Instance.dataManager; } }
    public static ResourceManager Resource {  get { return Instance.resourceManager; } }
    public static UIManager UI { get { return Instance.uIManager; } }

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

    private void Awake()
    {
        Init();

        Data.Init();
    }
}
