using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
   public class HeroData
    {
        public string type;
        public float health;
        public int damage;
        public int armor;
        public string attackMode;
        public float attackSpeed;
        public float moveSpeed;
        public int goldOnDeath;
        public int goldToBuy;
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
    //private const float delayAttack=0.01f;
    private GoldLoader goldAmountText;

    //indicate if one of the team hits the wall of the opponent

    //MESH
    private NavMeshAgent agent;

    //enemy Id to chase after
    private int enemyId;

    //enemies to chase after
    private GameObject[] enemies;

    //current chasing enemy
    private GameObject currentEnemy;
    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        moveable = true;
        teams = new string[2];
        teams[0]="LEFT" ;
        teams[1]="RIGHT";
        enemy = getEnemy();

        //load data of the hero
        string data = heroData.text;
        dataHero = JsonUtility.FromJson<HeroData>(data);

        if (dataHero.type.Equals ("MICKEY"))
        {
            PlayerPrefs.SetInt("MICKEY_goldToBuy", dataHero.goldToBuy);
        }
        else if (dataHero.type.Equals("RALPH"))
        {
            PlayerPrefs.SetInt("RALPH_goldToBuy", dataHero.goldToBuy);
        }
        //initially reach the random enemy
        enemies = GameObject.FindGameObjectsWithTag(enemy);
        enemyId = findRandomEnemy();
        currentEnemy = enemies[enemyId];
        agent.SetDestination(enemies[enemyId].transform.position);
    }

    // Update is called once per frame
    public void Update()
    {
        
        if (hitWall())
        {
            Debug.Log("hit the wall");
            Destroy(gameObject);
        }
        
        //only move the character when it's allowed
        if (moveable)
        {
            agent.isStopped = false;
            //check if he reach the destination
            if (agent.remainingDistance <= 0)
            {
                //find another object to attack
                enemies = GameObject.FindGameObjectsWithTag(enemy);
                enemyId = findRandomEnemy();
                if (enemyId < 0)
                {
                    agent.SetDestination(wall.transform.position);
                }
                else
                {
                    agent.SetDestination(enemies[enemyId].transform.position);
                }
            }

        }
        //it is attacking
        if(!moveable)
        {
            //stop moving
            agent.isStopped = true ;

            //we received information about the enemy, we can now attack them
            if (dataEnemy != null)
            {
                //deduct its health
                dataEnemy.health -=  (dataHero.damage * dataHero.attackSpeed *Time.deltaTime);
                Debug.Log("Health:" + dataEnemy.health);
            }
            //if the enemy health falls below zero, we exterminate it, and can move to find another enemy
            //and we need to add the gold we achieve by killing it
            if (dataEnemy.health <= 0)
            {
                Destroy(enemyObject);
                //now you need to move to find another opponent
                moveable = !moveable;
                //add gold collected
                addGoldCollected(dataEnemy.goldOnDeath);

                Debug.Log("DIE!!!");
                
            }
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        //it is an impact, stop moving, and attack the enemy
        if (other.transform.name.Equals(enemy))
        {
            moveable = false;
            //get the information of the enemy
            dataEnemy = other.gameObject.GetComponent<Hero>().getHeroData();
            //need to make them face to face literally, PENDINGGGG
            
        }
    }
    public string getEnemy()
    {
        if (team.Equals(teams[0]))
        {
            return teams[1];
        }
        else
            return teams[0];
    }

    public HeroData getHeroData()
    {
        return dataHero;
    }

    public void addGoldCollected(int gold)
    {
        goldAmountText = FindObjectOfType<GoldLoader>();
        goldAmountText.addGold(gold);
    }
    
    public bool hitWall()
    {
        //Wall at the right
        if (wall.transform.position.x < 0)
        {
            if (transform.position.x - 1.5f <= wall.transform.position.x)
            {
                return true;
            }
            return false;
        }
        //Wall at the left otherwise
        else
        {
            if (transform.position.x + 1.5f >= wall.transform.position.x)
            {
                return true;
            }
            return false;
        }
    }
    public float getMovespeed()
    {
        return dataHero.moveSpeed;
    }
    public int findRandomEnemy()
    {
        Debug.Log("Inside find randomenemy");
        //identify enemy objects
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemy);
        Debug.Log("Number of enemies team " + team + "found:" + enemies.Length);
        //we don't have any enemies to fight
        if (enemies.Length == 0)
        {
            return -1;
        }
        int randomId = UnityEngine.Random.Range(0, enemies.Length);
        Debug.Log("Team "+team+"found enemy index" + randomId);
        return randomId;
    }

    public bool reachDestination()
    {
        
        if(Math.Abs(Math.Abs(agent.transform.position.x)- Math.Abs(currentEnemy.transform.position.x)) <= 2.5f)
        {
            Debug.Log("Reach destination");
            return true;    
        }
        return false;
    }
}
