using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectPrefs 
{
    public static IntPref CoinsAmount;
    public static IntPref KeysAmount;

    static ProjectPrefs()
    {
        CoinsAmount = new IntPref("Coins", 0);
        KeysAmount = new IntPref("Keys", 0);
    }
}
