using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Image loadingImage;
    [SerializeField] private Text loadingText;

    [SerializeField] private List<Image> images;
    [SerializeField] private List<string> tips;

    //public void SceneLoading(string sceneName)
    //{
    //    blackScreen.DOFade(1, 1f).OnStart(() =>
    //    {
    //        blackScreen.blocksRaycasts = true; 
    //    }).OnComplete(() =>
    //    {
    //        AssetLoading(sceneName);
    //    });
    //}

    //private void AssetLoading(string sceneName)
    //{
    //    loadingUI.SetActive(true);
    //    MainManager.Addressable.LoadAsyncAll<Object>(sceneName, (key, cur, total) =>
    //    {
    //        Debug.Log($"{key} {cur}/{total}");

    //        if(total == cur)
    //            StartCoroutine(SceneLoad(sceneName));
    //    });
    //}

    //private IEnumerator SceneLoad(string sceneName)
    //{
    //    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
    //    while(op.isDone == false)
    //    {
    //        yield return null;
    //    }
        
    //    loadingUI.SetActive(false);
    //    blackScreen.DOFade(0, 1f).OnStart(() => { blackScreen.blocksRaycasts = false; });
    //}
}
