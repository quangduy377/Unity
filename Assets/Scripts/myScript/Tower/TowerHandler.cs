using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHandler : MonoBehaviour
{
    private float currentHp;
    private TowerData towerData;
    public string team;

    public GameObject healthBar;
    /// ///////////////////////NEW



    void Start()
    {
        if (team.Equals("RIGHT"))
        {    
            towerData = JsonUtility.FromJson<TowerData>(GameLoader.Instance.RightTower.text);
        }
        else
        {
            towerData = JsonUtility.FromJson<TowerData>(GameLoader.Instance.LeftTower.text);
        }
        currentHp = towerData.health;
        healthBar.GetComponent<Slider>().maxValue = towerData.health;
        healthBar.GetComponent<Slider>().minValue = 0.0f;
        healthBar.GetComponent<Slider>().value = healthBar.GetComponent<Slider>().maxValue;

    }
    // Update is called once per frame
    void Update()
    {
        //only set the fire when it is attacked   
        if (gameObject!=null && currentHp<=0)
        {
            Destroy(gameObject);
            Destroy(healthBar);
        }
    }
    public float getCurrentHp()
    {
        return currentHp;
    }
    public void setCurrentHp(float hp)
    {
        currentHp = hp;
    }
    public TowerData getTowerData()
    {
        return towerData;
    }
    

    
}
