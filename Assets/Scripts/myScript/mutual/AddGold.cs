using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGold
{
    public static void addGold(int gold, string name)
    {
        GameObject[] goldObject = GameObject.FindGameObjectsWithTag("GOLD");
        GoldLoader[] goldAmountText = new GoldLoader[2];
        for (int i = 0; i < goldObject.Length; i++)
        {
            goldAmountText[i] = goldObject[i].GetComponent<GoldLoader>();
            if (goldAmountText[i].type.Equals(name))
            {
                goldAmountText[i].addGold(gold);
                break;
            }
        }
    }
}
