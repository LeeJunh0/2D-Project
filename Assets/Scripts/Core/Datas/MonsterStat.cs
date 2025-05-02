using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterStat
{
    private string name;
    private int level;
    private float coinDefault;
    private float coinCoefficient;

    public int Level 
    { 
        get { return level; } 
        set 
        {
            level = value;
            coinDefault = MainManager.Data.MonsterLevelDict[name][level].coinDefault;
            coinCoefficient = MainManager.Data.MonsterLevelDict[name][level].coinCoefficient;
        }
    }

    public string Name { get { return name; } set { name = value; } }
    public float CoinDefault { get => coinDefault; } 
    public float CoinCoefficient { get => coinCoefficient; }
}