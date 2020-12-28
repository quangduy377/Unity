using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHandler : MonoBehaviour
{
    private float currentHp;
    private TowerData towerData;
    public string team;
    // Start is called before the first frame update
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
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject!=null && currentHp<=0)
        {
            Destroy(gameObject);
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
