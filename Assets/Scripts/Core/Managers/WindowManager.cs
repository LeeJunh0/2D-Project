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
    private readonly int windowVisble = 0x10000000;
    private readonly IntPtr level = new IntPtr(-1);
    private readonly uint showWindow = 0x0040;


    [SerializeField] int gameSizeX;
    [SerializeField] int gameSizeY;

    private int screenX;
    private int screenY;
    private Resolution curSize;

    private EWindowPos type = EWindowPos.Bottom;
    public EWindowPos Type { get => type; set { type = value; UpdateWindowPos(); } }

    private void Awake()
    {
        #if UNITY_EDITOR
        #else
        Type = EWindowPos.Bottom;
        #endif

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        curSize = Screen.currentResolution;

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Type = EWindowPos.Top;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Type = EWindowPos.Bottom;
    }

    private void UpdateScreenSize()
    {
        curSize = Screen.currentResolution;
        gameSizeX = curSize.width;
        gameSizeY = 400;

        switch (type)
        {
            case EWindowPos.Top:
                screenX = 0;
                screenY = 0;
                break;
            case EWindowPos.Bottom:
                screenX = 0;
                screenY = curSize.height - 400;
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
