using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    [Header("격자맵 관련")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private float startX;
    [SerializeField] private float startY;
    [SerializeField] bool isGridEnabled = false;
    [SerializeField] private List<GameObject> grids;

    [Header("레터박스 카메라")]
    [SerializeField] private Camera tabCamera;

    private void Awake()
    {
        grids = new List<GameObject>();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 46; j++)
            {
                GameObject grid = Instantiate(prefab);
                grid.transform.position = new Vector3(startX + (j * 1), startY + (i * 1), 0);
                grid.transform.SetParent(transform);
                grid.SetActive(false);
                grids.Add(grid);
            }
        }
    }

    public void SetGrid()
    {
        isGridEnabled = !isGridEnabled;

        if (isGridEnabled == true)
        {
            tabCamera.depth = 1;
            Camera.main.depth = 0;

            for (int i = 0; i < grids.Count; i++)
                grids[i].SetActive(true);
        }
        else
        {
            tabCamera.depth = 0;
            Camera.main.depth = 1;

            for (int i = 0; i < grids.Count; i++)
                grids[i].SetActive(false);
        }
    }
}
