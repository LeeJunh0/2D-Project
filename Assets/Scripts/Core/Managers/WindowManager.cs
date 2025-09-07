using System;
using System.Runtime.InteropServices;
using UnityEngine;
using static Define;

public class WindowManager : Singleton<WindowManager>
{
    public struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
    [DllImport("Dwmapi.dll")] private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref RECT pvParam, uint fWinIni);

    private readonly int GWL_STYLE = -16;
    private readonly int windowVisble = 0x10000000;

    private IntPtr curPin;
    private readonly IntPtr topMost = new IntPtr(-1);
    private readonly IntPtr noneTopMost = new IntPtr(-2);

    private readonly int showWindow = 0x0040;

    private readonly int GWL_EXSTYLE = -20;
    private readonly uint WS_EX_LAYERED = 0x00080000;
    private readonly uint SPI_GETWORKAREA = 0x0030;

    private readonly int WS_BORDER = 0x00800000;
    private readonly int WS_CAPTION = 0x00C00000;


    [SerializeField] private int gameSizeX;
    [SerializeField] private int gameSizeY;

    private int screenX;
    private int screenY;
    private Resolution curSize;

    private EWindowPos type = EWindowPos.Bottom;
    private bool isPin = true;
    public EWindowPos Type { get => type; set { type = value; UpdateWindowPos(); } }
    public bool IsPin
    {
        get => isPin;
        set
        {
            isPin = value;
            if (isPin == true)
                curPin = topMost;
            else
                curPin = noneTopMost;

            #if UNITY_EDITOR
            #else
            UpdateWindowPos();
            #endif
        }
    }

    private void Start()
    {
        #if UNITY_EDITOR
        #else
        Type = EWindowPos.Bottom;
        #endif

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Type = EWindowPos.Top;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Type = EWindowPos.Bottom;
    }

    private void UpdateScreenSize()
    {
        RECT rect = new RECT();
        SystemParametersInfo(SPI_GETWORKAREA, 0, ref rect, 0);

        gameSizeX = rect.Right - rect.Left;
        gameSizeY = 400;

        switch (type)
        {
            case EWindowPos.Top:
                screenX = rect.Left;
                screenY = rect.Top;
                break;
            case EWindowPos.Bottom:
                screenX = rect.Left;
                screenY = rect.Bottom - gameSizeY;
                break;
        }
    }

    private void UpdateWindowPos()
    {
        IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

        ApplyTransparentAndBorderless(hwnd);

        UpdateScreenSize();
        SetWindowPos(hwnd, curPin, screenX, screenY, gameSizeX, gameSizeY, showWindow);
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
