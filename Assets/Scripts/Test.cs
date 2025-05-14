using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    public Transform pos;

    private void Awake()
    {
        gameObject.AddEvent((evt) =>
        {

        });
    }

    private void CreateMonster()
    {
        GameObject go = MainManager.Resource.Instantiate("AngryPig");
        go.transform.position = new Vector3(pos.position.x, go.transform.position.y, pos.position.z);
    }
}
