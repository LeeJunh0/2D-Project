
# 숲 친구들 : 배경화면 숲 속 친구들

### 게임 소개

"숲 친구들 : 배경화면 숲 속 친구들" 은 바탕화면의 일부를 차지하는 **방치형 게임**입니다. 새로운 친구들과 함께 아름다운 마을을 꾸미고, 다양한 숲 속 친구들을 만나보세요. 친구들이 여러분의 잔고를 두둑하게 만들어줄 거예요!


### 개발 목적

* **방치형 장르 구현 및 출시 경험:** 최근 재미있게 즐겼던 방치형 게임 장르를 직접 구현하고, 게임을 출시하는 경험을 쌓는 것을 목표로 합니다.


* **바탕화면 투명화 기능 구현:** 바탕화면 위에 창의 테두리를 제거하고 배경을 투명하게 만들어, 마치 바탕화면의 일부처럼 보이게 하는 신기한 기능을 직접 구현해보고 싶었습니다.

### 핵심 코드
```c#
private void UpdateWindowPos()
{
    IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

    ApplyTransparentAndBorderless(hwnd);

    UpdateScreenSize();
    SetWindowPos(hwnd, curPin, screenX, screenY, gameSizeX, gameSizeY, showWindow);
}

private void ApplyTransparentAndBorderless(IntPtr hWnd)
{
    SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);

    int style = GetWindowLong(hWnd, GWL_STYLE);
    style &= ~WS_BORDER;
    style &= ~WS_CAPTION;
    SetWindowLong(hWnd, GWL_STYLE, style);

    MARGINS margins = new MARGINS { leftWidth = -1 };
    DwmExtendFrameIntoClientArea(hWnd, ref margins);
}
```

