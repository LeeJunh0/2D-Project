using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public float level;
    public float moveSpeed;
    public float coinDefault;
    public float coinCoefficient;

    public float Coin { get { return coinDefault * (level * coinCoefficient); } }
}
