using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MonsterGacha
{
    private static int CompareWeight(KeyValuePair<Define.EMonster_Rarity, float> a, KeyValuePair<Define.EMonster_Rarity, float> b)
    {
        return a.Value > b.Value ? -1 : 1;
    }

    public static Define.EMonster_Rarity RarityRandom()
    {
        Dictionary<Define.EMonster_Rarity, float> weightTable = new Dictionary<Define.EMonster_Rarity, float>()
        {
            { Define.EMonster_Rarity.Legend, 0.1f },
            { Define.EMonster_Rarity.Unique, 0.9f },
            { Define.EMonster_Rarity.Rare, 2f }, 
            { Define.EMonster_Rarity.Normal, 7f }
        };

        float total = 0f;
        foreach(float weight in weightTable.Values)
            total += weight;

        float pivot = UnityEngine.Random.Range(0, total);
        float totalPercent = 0;

        foreach(var weight in weightTable)
        {
            totalPercent += weight.Value;
            if (pivot <= totalPercent)
            {
                Extension.SuccessLog($"{weight.Key}");
                return weight.Key;
            }
        }

        return Define.EMonster_Rarity.None;
    }

    #if UNITY_EDITOR
    public static void Test()
    {
        Dictionary<Define.EMonster_Rarity, float> weightTable = new Dictionary<Define.EMonster_Rarity, float>()
        {
            { Define.EMonster_Rarity.Legend, 1f },
            { Define.EMonster_Rarity.Unique, 99f }
        };

        float total = 0f;
        foreach (float weight in weightTable.Values)
            total += weight;

        Dictionary<Define.EMonster_Rarity, int> counts = new Dictionary<Define.EMonster_Rarity, int>();
        for (int i = 0; i < 10000; i++)
        {
            float pivot = UnityEngine.Random.Range(0, total);
            //float curPersent = 0f;
            bool isCheck = false;
            foreach (var weight in weightTable)
            {
                //curPersent += weight.Value;

                if (pivot <= weight.Value)
                {
                    if (counts.ContainsKey(weight.Key) == true)
                        counts[weight.Key] += 1;
                    else
                        counts.Add(weight.Key, 1);

                    isCheck = true;
                    break;
                }
            }

            if (isCheck == true)
                Extension.SuccessLog($"성공 {i} 번째 : pivot {pivot}");
            else
                Extension.ErrorLog($"실패 {i} 번째 : pivot {pivot}");
        }

        int sum = 0;
        foreach (var weight in counts)
        {
            sum += weight.Value;
            Extension.SuccessLog($"{weight.Key} : {weight.Value} / {Math.Round((float)weight.Value / 10000, 2)}");
        }

        Extension.SuccessLog($"검증한 횟수 : {sum}");
    }
    #endif
}
