using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Image loadingImage;
    [SerializeField] private Text loadingText;

    [SerializeField] private List<Image> images;
    [SerializeField] private List<string> tips;
    [SerializeField] private float fakeSec;

    private void Awake()
    {
        AssetLoading("Title");
        DontDestroyOnLoad(gameObject);
    }

    private void AssetLoading(string sceneName)
    {
        MainManager.Addressable.LoadAsyncAll<Object>(sceneName, (key, cur, total) =>
        {
            cur++;
            Debug.Log($"{key} {cur}/{total}");

            if(total == cur)
                StartCoroutine(SceneLoading(sceneName));
        });
    }

    private IEnumerator SceneLoading(string sceneName)
    {
        yield return new WaitForSeconds(fakeSec);
        SceneManager.LoadScene(sceneName);
    }
}
