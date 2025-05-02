using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

static public class ExChanger
{
    // 단위
    static private string[] units = { "", "만", "억", "조", "경", "해", "자", "양", "구", "간" };

    static public string GoldToText(double gold)
    {
        if (gold < 10000000)
            return gold.ToString("N0") + "원";

        int unitIndex = 0;
        while(gold >= 10000)
        {
            gold /= 10000;
            unitIndex++;
        }

        return gold.ToString("0.##") + units[unitIndex] + "원";
    }
}
