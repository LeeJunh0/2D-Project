using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindAnyObjectByType<T>();

                if (instance == null)
                    Debug.LogError($"{typeof(T)}는 없는 싱글톤 매니저 입니다.");
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
