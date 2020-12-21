using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
   public class HeroData
    {
        public string type;
        public float health;
        public float damage;
        public float armor;
        public string attackMode;
        public float attackSpeed;
        public float moveSpeed;
        public float goldOnDeath;
        public float goldToBuy;
    }

    public GameObject wall;
    //LEFT OR RIGHT
    public string team; 


    private string enemy;

    // ['LEFT','RIGHT']
    private string[] teams; 
    private bool moveable;

    // passing a file data
    public TextAsset heroData; 

    //store original data of a hero
    private HeroData dataHero;
    private GameObject heroObject;

    private HeroData dataEnemy;
    private GameObject enemyObject;
    private const float delayAttack=0.01f;
    // Start is called before the first frame update
    void Start()
    {
        moveable = true;
        teams = new string[2];
        teams[0]="LEFT" ;
        teams[1]="RIGHT";
        enemy = getEnemy();


        //load data of the hero
        string data = heroData.text;
        dataHero = JsonUtility.FromJson<HeroData>(data);
    }

    // Update is called once per frame
    void Update()
    {
        //only move the character when it's allowed
        if (moveable)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
            new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z),
            dataHero.moveSpeed * Time.deltaTime);
        }
        //it is attacking
        else
        {
            //we received information about the enemy, we can now attack them
            if (dataEnemy != null)
            {
                //deduct its health
                dataEnemy.health -=  dataHero.damage * dataHero.attackSpeed *delayAttack;
                Debug.Log("Health:" + dataEnemy.health);
            }
            //if the enemy health falls below zero, we exterminate it, and can move to find another enemy
            if (dataEnemy.health <= 0)
            {
                Destroy(enemyObject);
                moveable = !moveable;
            }
                

        }
        
    }
    void OnCollisionEnter(Collision other)
    {
        //it is an impact, stop moving, and attack the enemy
        if (other.transform.name.Equals(enemy))
        {
            moveable = false;
            //get the information of the enemy
            enemyObject = other.gameObject;
            //now attack the enemy
            dataEnemy = other.gameObject.GetComponent<Hero>().getHeroData();
        }
        Debug.Log("On collision called");
    }
    string getEnemy()
    {
        if (team.Equals(teams[0]))
        {
            return teams[1];
        }
        else
            return teams[0];
    }

    HeroData getHeroData()
    {
        return dataHero;
    }
    
}
