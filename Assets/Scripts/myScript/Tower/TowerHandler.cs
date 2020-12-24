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
    // Start is called before the first frame update
    void Start()
    {
        string data = file.text;
        towerData = JsonUtility.FromJson<TowerData>(data);
        currentHp = towerData.health;
        oneSec = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        oneSec -= Time.deltaTime;
        
        if (isAttacked)
        {
            if (oneSec <= 0)
            {
                currentHp += towerData.armor;
                oneSec = 1.0f;
            }
            Debug.Log("Tower is being attacked");
            for(int i = 0; i < enemies.Count; i++)
            {
                if (attackerType.Equals("Hero"))
                {
                    damageToTower = enemies[i].GetComponent<Hero>().getHeroData().damage;
                    attackSpeed = enemies[i].GetComponent<Hero>().getHeroData().attackSpeed;
                }
                else
                {
                    damageToTower = enemies[i].GetComponent<Enemy>().getHeroData().damage;
                    attackSpeed = enemies[i].GetComponent<Enemy>().getHeroData().attackSpeed;
                }

                currentHp -= (damageToTower * attackSpeed * Time.deltaTime);
                
                Debug.Log("tower HP:"+currentHp);
                if (currentHp < 0)
                {
                    Destroy(gameObject);
                    isAttacked = false;
                    return;
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided:");
        

        //this is the right building
        if (team.Equals("RIGHT"))
        {
            //attacked from the left, OK, enemies or player
            if (collision.transform.tag.Equals("LEFT"))
            {

                enemies.Add(collision.gameObject);
                isAttacked = true;
                Debug.Log(enemies.Count + " attacking the building");

                if (PlayerPrefs.GetString("playerSide").Equals("LEFT"))
                {
                    attackerType = "Hero";
                }
                else
                {
                    attackerType = "Enemy";
                }

            }
        }
        //this is the left building
        else
        {
            //attacked from the right, OK, enemies
            if (collision.transform.tag.Equals("RIGHT"))
            {
                enemies.Add(collision.gameObject);
                isAttacked = true;
                Debug.Log(enemies.Count + " attacking the building");
                if (PlayerPrefs.GetString("playerSide").Equals("RIGHT"))
                {
                    attackerType = "Hero";
                }
                else
                {
                    attackerType = "Enemy";
                }
            }
        }
    }
}
