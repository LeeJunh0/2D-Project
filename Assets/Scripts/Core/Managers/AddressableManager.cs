using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class AddressableManager
{
    private Dictionary<string, Object> resourceDict = new Dictionary<string, Object>();
    private Dictionary<string, AsyncOperationHandle> handleDict = new Dictionary<string, AsyncOperationHandle>();

    public T Load<T>(string name) where T : Object
    {
        if (resourceDict.TryGetValue(name, out Object obj) == false)
        {
            Debug.LogError($"{name}은 로드된 데이터가 아닙니다.");
            return null;
        }

        return obj as T;
    }

    private void LoadAsync<T> (string key, Action<T> action) where T : Object
    {
        if(resourceDict.TryGetValue(key, out Object resource) == true)
        {
            action.Invoke(resource as T);
            return;
        }

        AsyncOperationHandle<T> asyncOp = Addressables.LoadAssetAsync<T>(key);
        asyncOp.Completed += (op) =>
        {
            if (resourceDict.ContainsKey(key) == false)
            {
                resourceDict.Add(key, op.Result);
                handleDict.Add(key, asyncOp);
            }
            
            action.Invoke(op.Result);
        };
    }

    public void LoadAsyncAll<T> (string key, Action<string, int, int> action) where T : Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(key, typeof(T));

        handle.Completed += (op) =>
        {
            int cur = 0;
            int total = op.Result.Count;

            foreach(IResourceLocation result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    cur++;
                    action.Invoke(result.PrimaryKey, cur, total);
                });
            }
        };
    }

    public void Clear()
    {
        resourceDict.Clear();

        foreach (var op in handleDict)
            Addressables.Release(op);

        handleDict.Clear();
    }
}
