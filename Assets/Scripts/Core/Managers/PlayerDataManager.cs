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

    public double Gold { get { return playerInfo.gold; }  set { playerInfo.gold = value; } }

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        if(File.Exists(path) == false)
        {
            Debug.Log("Not Find Data");
            CreateNewData();
            Debug.Log("New Data Create Complete");
        }

        string jsonData = File.ReadAllText(path);
        playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(jsonData);
        Debug.Log("PlayerData Load Complete");
    }

    private void CreateNewData()
    {
        playerInfo = new PlayerInfo();
        playerInfo.gold = 0;

        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Close();
        SaveData();
    }

    private void SaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        string jsonData = JsonConvert.SerializeObject(playerInfo, Formatting.Indented);
        File.WriteAllText(path, jsonData);
    }

    public void TextUpdate()
    {
        if (goldText == null)
            goldText = GameObject.Find("CoinText").GetOrAddComponent<TextMeshProUGUI>();

        goldText.text = ExChanger.GoldToText(playerInfo.gold);
    }
}
