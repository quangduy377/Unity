using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldLoader : MonoBehaviour
{
    private Text goldAmount;
    private GoldData originalGold;
    public TextAsset file;

    //original gold of player
    private int currentGold;
    
    //indicate the amount of time gold gets increased after
    private float second = 1.0f;

    public string type;
    // Start is called before the first frame update
    void Start()
    {
        string data = file.text;
        originalGold = JsonUtility.FromJson<GoldData>(data);
        currentGold = originalGold.basicGold;
        //find an appropriate box to render money
        //this is a player
        if (originalGold.type.Equals("Player"))
        {
            //get the box on the left please
            if (PlayerPrefs.GetString("playerSide").Equals("LEFT"))
            {
                goldAmount = GameObject.Find("playerGoldAmount").GetComponent<Text>();
            }
            //get the box on the right please
            else
            {
                goldAmount = GameObject.Find("enemyGoldAmount").GetComponent<Text>();

            }
        }
        //this is an enemy
        else
        {
            //get the box on the left please
            if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
            {
                goldAmount = GameObject.Find("playerGoldAmount").GetComponent<Text>();
            }
            //get the box on the right please
            else
            {
                goldAmount = GameObject.Find("enemyGoldAmount").GetComponent<Text>();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (second <= 0)
        {
            //when 1 second passes, we update the gold
            currentGold += originalGold.goldOnSecond;
            goldAmount.text = currentGold + "";
            second = 1.0f;
        }
        second -= Time.deltaTime;
    }

    public void addGold(int gold)
    {
        currentGold += gold;
    }
    public int getCurrentGold()
    {
        return currentGold;
    }
}
