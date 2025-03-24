using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private int sorted = 0;
    Stack<UI_Popup> uiStack = new Stack<UI_Popup>();

    public void ShowPopup<T>() where T : UI_Popup
    {
        GameObject go = MainManager.Resource.LoadPrefab<T>();
    }

    public void ClosePopup<T>() where T : UI_Popup
    {
        
    }

    
}
