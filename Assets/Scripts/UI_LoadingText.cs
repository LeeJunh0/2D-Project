using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LoadingText : MonoBehaviour
{
    private TextMeshProUGUI loadingText;

    private void Awake()
    {
        loadingText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(LoadingText());
    }

    private IEnumerator LoadingText()
    {
        string defaultText = "Loading";
        int max = 3;
        while(true)
        {
            loadingText.text = defaultText;
            for(int i = 0; i < max; i++)
            {
                loadingText.text += ".";
                yield return new WaitForSeconds(1f);
            }         
        }       
    }
}
