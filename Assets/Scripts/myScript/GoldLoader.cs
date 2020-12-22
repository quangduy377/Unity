using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldLoader : MonoBehaviour
{
    public class GoldData{
        public int basicGold;
        public int goldOnSecond;
        public string type;

    }

    private Text goldAmount;
    private GoldData originalGold;
    public TextAsset file;

    //original gold of player
    private int currentGold;
    
    //indicate the amount of time gold gets increased after
    private float second = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        string data = file.text;
        originalGold = JsonUtility.FromJson<GoldData>(data);
        currentGold = originalGold.basicGold;
        goldAmount = GetComponent<Text>();
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
}
