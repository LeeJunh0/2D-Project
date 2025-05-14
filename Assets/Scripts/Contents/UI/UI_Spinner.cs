using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Spinner : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    private RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        StartCoroutine(OnSpinner());
    }

    IEnumerator OnSpinner()
    {
        Sequence sequence = DOTween.Sequence();
        while(true)
        {
            sequence.Prepend(rect.DORotate(new Vector3(0, 0, rect.rotation.z + 360f), 1.5f, RotateMode.FastBeyond360));
            yield return new WaitForSeconds(2f);
        }      
    }
}
