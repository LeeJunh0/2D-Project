using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

static public class ExChanger
{
    // ����
    static private string[] units = { "", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

    static public string GoldToText(double gold)
    {
        if (gold < 10000000)
            return gold.ToString("N0") + "��";

        int unitIndex = 0;
        while(gold >= 10000)
        {
            gold /= 10000;
            unitIndex++;
        }

        return gold.ToString("0.##") + units[unitIndex] + "��";
    }
}
