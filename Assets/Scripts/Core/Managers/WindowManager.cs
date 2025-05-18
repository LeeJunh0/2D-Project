using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using static Define;

public class WindowManager : MonoBehaviour
{
    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private readonly int style = -16;
    private readonly uint windowPopup = 0x80000000;     
    private readonly int windowDisabled = 0x08000000;  
    private readonly int windowVisble = 0x10000000;
    private const uint showWindow = 0x0040;
    private readonly IntPtr level = new IntPtr(-1);

    [SerializeField] int gameSizeX = 600;
    [SerializeField] int gameSizeY;

    private int screenX;
    private int screenY;
    private Resolution curSize;

    private EWindowPos type = EWindowPos.Right;
    public EWindowPos Type { get => type; set { type = value; UpdateWindowPos(); } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        curSize = Screen.currentResolution;

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Type = EWindowPos.Left;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            Type = EWindowPos.Right;
    }

    private void UpdateScreenSize()
    {
        curSize = Screen.currentResolution;
        gameSizeX = 600;
        gameSizeY = curSize.height;

        switch (type)
        {
            case EWindowPos.Left:
                screenX = 0;
                screenY = 0;
                break;
            case EWindowPos.Right:
                screenX = curSize.width - gameSizeX;
                screenY = 0;
                break;
        }
    }

    private void UpdateWindowPos()
    {
        IntPtr hwnd = GetActiveWindow();

        UpdateScreenSize();
        SetWindowLong(hwnd, style, windowVisble);
        SetWindowPos(hwnd, level, screenX, screenY, gameSizeX, gameSizeY, showWindow);
    }
}
