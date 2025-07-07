using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager instance = null;

    private static MainManager Instance { get { Init(); return instance; } }

    // Not MonoBehaviour
    private DataManager dataManager = new DataManager();
    private ResourceManager resourceManager = new ResourceManager();
    private UIManager uIManager = new UIManager();
    private AddressableManager addressableManager = new AddressableManager();

    public static DataManager Data { get => Instance.dataManager; }
    public static ResourceManager Resource { get => Instance.resourceManager; }
    public static UIManager UI { get => Instance.uIManager; }
    public static AddressableManager Addressable { get => Instance.addressableManager; }

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

        Addressable.LoadAsyncAll<Object>("Game", (key, cur, total) =>
        {
            Extension.LoadingLog($"{key} {cur}/{total}");
            if(total == cur)
            {
                Data.Init();
            }
        });       
    }
}
