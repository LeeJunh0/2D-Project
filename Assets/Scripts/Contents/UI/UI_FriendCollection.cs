using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_FriendCollection : MonoBehaviour
{
    private const int RarityCount = 4;
    private List<UI_CollectionSlot> slots;

    [SerializeField] private GameObject ui;
    [SerializeField] private Button exitButton;
    [SerializeField] private Transform content;
    private void Start()
    {
        UI_Game.CollectionOpenHandler -= OnCollection;
        UI_Game.CollectionOpenHandler += OnCollection;

        exitButton.gameObject.AddEvent(OffCollection);
        slots = new List<UI_CollectionSlot>();
    }

    private void OnCollection()
    {
        ui.SetActive(true);
        CollectionInit();
    }

    private void OffCollection(PointerEventData eventData)
    {
        ui.SetActive(false);
    }

    private void CollectionInit()
    {
        Dictionary<string, FriendCollectionInfo> dict = PlayerDataManager.Instance.collection;

        Define.EFriend_Rarity[] rarites =
        {
            Define.EFriend_Rarity.Normal,
            Define.EFriend_Rarity.Rare,
            Define.EFriend_Rarity.Named,
            Define.EFriend_Rarity.Boss
        };

        foreach (var info in dict)
        {
            for (int i = 0; i < rarites.Length; i++)
            {
                GameObject go = MainManager.Resource.Instantiate("Collection_Slot", content);
                UI_CollectionSlot slot = go.GetComponent<UI_CollectionSlot>();
                slot.Init(info.Key, rarites[i]);
                slots.Add(slot);
            }
        }
    }

    private void HandlerClear()
    {
        UI_Game.CollectionOpenHandler -= OnCollection;
    }

    private void OnApplicationQuit()
    {
        HandlerClear();
    }
}
