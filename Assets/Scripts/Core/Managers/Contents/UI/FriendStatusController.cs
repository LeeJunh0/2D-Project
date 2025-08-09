using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// 친구들의 능력치 관리 및 관련 UI관리
/// 나중에 커지게 되면 둘이 분리할수도..?
/// 
/// </summary>
public class FriendStatusController : Singleton<FriendStatusController>
{
    [SerializeField] private GameObject renderTexture;
    [SerializeField] private TextMeshProUGUI friendCountText;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Transform listContent;

    private List<UI_FriendListSlot> friendListSlots;

    private UI_FriendListSlot curFriend;

    public void FriendListUpdate()
    {

    }
}
