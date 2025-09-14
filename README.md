
# 숲 친구들 : 배경화면 숲 속 친구들

### 게임 소개

"숲 친구들 : 배경화면 숲 속 친구들" 은 바탕화면의 일부를 차지하는 **방치형 게임**입니다. 새로운 친구들과 함께 아름다운 마을을 꾸미고, 다양한 숲 속 친구들을 만나보세요. 친구들이 여러분의 잔고를 두둑하게 만들어줄 거예요!


### 개발 목적

* **방치형 장르 구현 및 출시 경험:** 최근 재미있게 즐겼던 방치형 게임 장르를 직접 구현하고, 게임을 출시하는 경험을 쌓는 것을 목표로 합니다.


* **바탕화면 투명화 기능 구현:** 바탕화면 위에 창의 테두리를 제거하고 배경을 투명하게 만들어, 마치 바탕화면의 일부처럼 보이게 하는 신기한 기능을 직접 구현해보고 싶었습니다.

### 배경 투명화, 창 속성제어
* P/Invoke기술을 통해 WinAPI의 기능들로 창을 제어하는 방식으로 투명화 및 깔끔한 창을 구현
<details><summary>코드</summary>
    
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
</details>
<br>

### UI 상호작용 및 데이터관리
- 기존에는 싱글톤 및 인스팩터창을 통한 직접적인 호출로 각 UI 혹은 데이터들의 메서드들 혹은 변수들을 제어하는 방식을 사용
- 연결되는 팝업창 혹은 데이터가 많아지면서 강한 결합으로 기능추가와 수정에 한계를 느낌
- 기능을 가지는 클래스에서 기능을 사용하는 클래스의 존재를 몰라도 되도록 이벤트 기반으로 구현

<details>
    <summary>코드</summary>

 ```c#
public static Action<string, UnlockActionType> OnFriendBuyHandler;
public static Action<int, UnlockActionType> OnFriendSellHandler;
public static Action<string> OnUnLockSlotHandler;
public static Action OnFriendCountUpdateHandler;
public static Action<string> OnGachaUpdateHandler;

public static void UnLockActionBuy(string name)
{
    OnFriendBuyHandler?.Invoke(name, UnlockActionType.Buy);
}

public static void UnLockActionSell(int index)
{
    OnFriendSellHandler?.Invoke(index, UnlockActionType.Sell);
}

public static void UnLockSlotUI(string name)
{
    OnUnLockSlotHandler?.Invoke(name);
}

public static void FriendCountUpdate()
{
    OnFriendCountUpdateHandler?.Invoke();
}

public static void GachaUpdate(string name)
{
    OnGachaUpdateHandler?.Invoke(name);
}
```

</details>

### 부모UI의 이벤트차단 처리
- 부모UI인 스크롤뷰안에 버튼 오브젝트인 자식UI를 생성 후 문제발생
- 부모 UI에게 가야할 이벤트를 자식오브젝트에서 차단하는것을 확인
- 스크롤뷰 부모를 가진 자식UI들에게 차단된 이벤트를 부모에게 재전파하도록 구현

<details>
    <summary>코드</summary>

```c#
public abstract class UI_ScrollInButton : MonoBehaviour
{
    private ScrollRect parentScroll;

    protected virtual void SetEvent()
    {
        parentScroll = gameObject.FindParent<ScrollRect>();

        gameObject.AddEvent(OnBeginDrag,Define.EEvent_Type.BeginDrag);
        gameObject.AddEvent(OnDrag,Define.EEvent_Type.Drag);
        gameObject.AddEvent(OnEndDrag,Define.EEvent_Type.EndDrag);
    }

    protected virtual void OnBeginDrag(PointerEventData eventData)
    {
        parentScroll.OnBeginDrag(eventData);
    }

    protected virtual void OnDrag(PointerEventData eventData)
    {
        parentScroll.OnDrag(eventData);
    }

    protected virtual void OnEndDrag(PointerEventData eventData)
    {
        parentScroll.OnEndDrag(eventData);
    }
}
```
</details>
