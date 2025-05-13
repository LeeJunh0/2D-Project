using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RevenueObject : MonoBehaviour
{
    [SerializeField] private Vector3 initPos;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI goldText;

    public void CreateRevenue(double gold)
    {
        InitUI();
        goldText.text = ExChanger.GoldToText(gold);
        OnRevenueText();
    }
0
    private void InitUI()
    {
        group.alpha = 1;
        transform.position = transform.parent.position + initPos;
    }

    private void OnRevenueText()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Join(transform.DOMoveY(transform.parent.position.y + 0.6f, 1.5f));
        sequence.Join(group.DOFade(0, 1.5f));
    }
}
