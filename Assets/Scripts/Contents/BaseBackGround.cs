using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseBackGround : MonoBehaviour
{
    private const float CloudMaxX = 47f;

    [SerializeField] private List<GameObject> cloudList;
    [SerializeField] private float cloudMoveSpeed;
    private Coroutine coroutine;

    private void Awake()
    {
        coroutine = StartCoroutine(CloudMove());
    }

    private IEnumerator CloudMove()
    {
        while (Application.isPlaying == true)
        {
            for (int i = 0; i < cloudList.Count; i++)
            {
                cloudList[i].transform.Translate(Vector3.right * Time.deltaTime * cloudMoveSpeed);
                if (cloudList[i].transform.position.x >= CloudMaxX)
                {
                    Vector3 pos = cloudList[i].transform.position;
                    pos.x = -CloudMaxX;
                    cloudList[i].transform.position = pos;
                }
            }
            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        StopCoroutine(coroutine);
    }
}
