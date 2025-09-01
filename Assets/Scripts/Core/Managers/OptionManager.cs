using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class OptionManager : Singleton<OptionManager>
{
    public const float MaxBGM = 1f;
    public const float MaxEffect = 1f;

    [SerializeField] private PlayerOptionData optionData;
    [SerializeField] private float curBGM;
    [SerializeField] private float curEffect;
    [SerializeField] private bool isMute;
    [SerializeField] private bool isWindowPin;

    public float CurBGM
    {
        get => curBGM;
        set
        {
            curBGM = Mathf.Clamp(value, 0f, MaxBGM);
            AudioManager.Instance.SetBGMVolum(curBGM);
        }
    }
    public float CurEffect
    {
        get => curEffect;
        set
        {
            curEffect = Mathf.Clamp(value, 0f, MaxEffect);
            AudioManager.Instance.SetEffectVolume(curEffect);
        }
    }
    public bool IsMute
    {
        get => isMute; set
        {
            isMute = value;
            AudioManager.Instance.SetMute(IsMute);
        }
    }
    public bool IsWindowPin { get => isWindowPin; set => isWindowPin = value; }

    private void Start()
    {
        LoadOptionData();

        CurBGM = optionData.curBGM;
        CurEffect = optionData.curEffect;
        IsMute = optionData.isMute;
        IsWindowPin = optionData.isWindowPin;
        AudioManager.Instance.Init();
    }

    private void LoadOptionData()
    {
        string path = Path.Combine(Application.persistentDataPath, "OptionData.json");
        if (File.Exists(path) == false)
        {
            Extension.ErrorLog("Not Find OptionData");
            optionData = new PlayerOptionData();
        }
        else
        {
            string jsonData = File.ReadAllText(path);
            optionData = JsonConvert.DeserializeObject<PlayerOptionData>(jsonData);
        }
    }

    public void SaveOptionData()
    {
        optionData.curBGM = CurBGM;
        optionData.curEffect = CurEffect;
        optionData.isMute = IsMute;
        optionData.isWindowPin = IsWindowPin;

        string path = Path.Combine(Application.persistentDataPath, "OptionData.json");
        string jsonData = JsonConvert.SerializeObject(optionData, Formatting.Indented);
        File.WriteAllText(path, jsonData);
        Extension.SuccessLog("OptionData Save Complete");
    }
}
