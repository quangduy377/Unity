using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHandler : MonoBehaviour
{
    public TextAsset file;
    private float currentHp;
    private TowerData towerData;
    private bool isAttacked;
    private int damageToTower;
    private float attackSpeed;
    private List<GameObject> enemies = new List<GameObject>();


    private string[] teams = new string[] { "LEFT", "RIGHT" } ;
    public string team;

    private float oneSec;

    private string attackerType;

    private float timeCounter;
    private float interval;
    // Start is called before the first frame update
    void Start()
    {
        string data = file.text;
        towerData = JsonUtility.FromJson<TowerData>(data);
        currentHp = towerData.health;
        oneSec = 1.0f;
        interval = 0;
        timeCounter = 0;
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
