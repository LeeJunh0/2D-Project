using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static Define;
using static WindowManager;

public class WindowManager : MonoBehaviour
{
    public struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;
    }

    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
    [DllImport("Dwmapi.dll")] private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    private readonly int GWL_STYLE = -16;
    private readonly int windowVisble = 0x10000000;
    private readonly IntPtr level = new IntPtr(-1);
    private readonly int showWindow = 0x0040;

    private readonly int GWL_EXSTYLE = -20;
    private readonly uint WS_EX_LAYERED = 0x00080000;

    private readonly int WS_BORDER = 0x00800000;
    private readonly int WS_CAPTION = 0x00C00000;


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
                screenY = curSize.height - gameSizeY;
                break;
        }
    }

    private void UpdateWindowPos()
    {
        IntPtr hwnd = GetActiveWindow();

        ApplyTransparentAndBorderless(hwnd);

        UpdateScreenSize();
        SetWindowPos(hwnd, level, screenX, screenY, gameSizeX, gameSizeY, showWindow);
    }
   
    private void ApplyTransparentAndBorderless(IntPtr hWnd)
    {
        SetWindowLong(hWnd, GWL_EXSTYLE, (int)WS_EX_LAYERED);

        int style = GetWindowLong(hWnd, GWL_STYLE);
        style &= ~WS_BORDER;
        style &= ~WS_CAPTION;
        SetWindowLong(hWnd, GWL_STYLE, style);

        MARGINS margins = new MARGINS { leftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
    }
}
