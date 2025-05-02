using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    [Header("플레이어 저장 데이터")]
    [SerializeField] private PlayerInfo playerInfo;

    [SerializeField] private TextMeshProUGUI goldText;
    private string path = Path.Combine(Application.dataPath, "Resources", "JsonDatas", "PlayerData.json");

    public double Gold { get { return playerInfo.gold; }  set { playerInfo.gold = value; } }

    private void Awake()
    {
        LoadData();
        SaveData();
        TextUpdate();
    }

    private void LoadData()
    {
        playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(MainManager.Data.JsonLoad("PlayerData"));

        if (playerInfo == null) 
        {
            playerInfo = new PlayerInfo();
            playerInfo.gold = 0;
        }
    }

    private void SaveData()
    {
        string jsonData = JsonConvert.SerializeObject(playerInfo, Formatting.Indented);
        File.WriteAllText(path, jsonData);
    }

    public void TextUpdate()
    {
        goldText.text = ExChanger.GoldToText(playerInfo.gold);
    }
}
