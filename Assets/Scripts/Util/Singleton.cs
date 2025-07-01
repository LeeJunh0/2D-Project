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
                    Debug.LogError($"{typeof(T)}�� ���� �̱��� �Ŵ��� �Դϴ�.");
            }

            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
