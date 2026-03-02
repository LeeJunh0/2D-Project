using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    // Not MonoBehaviour
    private DataManager dataManager = new DataManager();
    private ResourceManager resourceManager = new ResourceManager();
    private AddressableManager addressableManager = new AddressableManager();

    public static DataManager Data { get => Instance.dataManager; }
    public static ResourceManager Resource { get => Instance.resourceManager; }
    public static AddressableManager Addressable { get => Instance.addressableManager; }

    protected override void Awake()
    {
        base.Awake();

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
